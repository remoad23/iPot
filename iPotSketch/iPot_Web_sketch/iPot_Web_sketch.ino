#include <SPI.h>
#include <Ethernet.h>

/*
 * iPot
 */

//pins
int pumpPin = A0; // pin to control the waterpump
int photoresistorPin = A1; //sensor pin for the brightness
int moisturePin = A2; //sensor pin for the moisture of the plant
int liquidLevelPin = 5; //sensor pin for the liquid level sensor 
int ledPin = 9; // pin to control the brightness of the leds

//pin values
int pumpDuration = 7; //duration of the waterpumps motor in seconds
int photoresistorValue = 0; //sensor value of the photoresistor
int moistureValue = 0; //sensor value of the mousture sensor
int liquidLevelValue = 0; // sensor value of the liquid level sensor
int ledBrightness = 0; //brightness value of the leds
int ledBrightnessOffset = 20; //offset for the brightness of the leds

//control delay
int delayBetweenControlCycles = 10; // in seconds

//set values by the user from the webserver
int moistureMinimum = 600; //pumping starts at this value
int lightMinimum = 1023; //leds are activated at this value
bool automaticWatering = true; //determines if the plant should be autmatically watered
bool automaticLight = true; //determines if the plant should be automatically lighted

//Webserver values
byte mac[] = { 0xDE, 0xAD, 0xBE, 0xEF, 0xFE, 0xED };

EthernetClient client;

int    HTTP_PORT   = 5000;
char   HOST_NAME[] = "192.168.2.211"; // change to webserver ip


void setup() {
  Serial.begin(19200);
  //input pins
  pinMode(photoresistorPin, INPUT);
  pinMode(moisturePin, INPUT);
  pinMode(liquidLevelPin, INPUT);

  //output pins
  pinMode(ledPin, OUTPUT);
  pinMode(pumpPin, OUTPUT);

  //Ethernet connection
  if (Ethernet.begin(mac) == 0) {
    Serial.println("Failed to obtaining an IP address using DHCP");
    while(true);
  }
}

void loop() {
  readServerValues();
  measurePlantState();
  
  
  if(automaticLight && lightMinimum > photoresistorValue){
    changeLedBrightness();
  }
  else{
    turnOffLeds();
  }
  if(automaticWatering && moistureMinimum > moistureValue){
    if(liquidLevelValue == 1){
      pumpWater();
    }
    
  }

  sendSensorValuesToServer();

  delay(delayBetweenControlCycles * 1000);

}

//Measures the values of the sensors
void measurePlantState(){
  photoresistorValue = analogRead(photoresistorPin);
  moistureValue = analogRead(moisturePin);
  liquidLevelValue = digitalRead(liquidLevelPin);
  Serial.print("photo: ");
  Serial.print(photoresistorValue);
  Serial.print(", moist: ");
  Serial.print(moistureValue);
  Serial.print(", level: ");
  Serial.println(liquidLevelValue);
}

//Changes the brightness of the leds depending on the measured light intensity
void changeLedBrightness(){
  ledBrightness = photoresistorValue / 4; // divided by 4 because output range is 0-255 and input range is 0-1023
  int ledWithOffset = ledBrightness + ledBrightnessOffset;
  if(ledWithOffset > 255){
    analogWrite(ledPin, 255);
    Serial.print("led: ");
    Serial.println(255);
  }
  else{
    analogWrite(ledPin, ledWithOffset);
    Serial.print("led: ");
    Serial.println(ledWithOffset);
  }
  Serial.flush();
  
}

//turns the leds off
void turnOffLeds(){
  analogWrite(ledPin, 0);
}

//pumps water
void pumpWater(){
  digitalWrite(pumpPin, HIGH);
  delay(pumpDuration * 1000);
  digitalWrite(pumpPin,LOW);
}

//reads the set values from the webserver
void readServerValues(){
  if(client.connect(HOST_NAME, HTTP_PORT)) {
    // if connected:
    Serial.println("Connected to server");
    // make a HTTP request:
    // send HTTP header
    client.println("GET /GetPlantSettings HTTP/1.1");
    client.println("Host: " + String(HOST_NAME));
    client.println("Connection: close");
    client.println(); // end HTTP header
    
    char response[4][8] = {"","","",""};
    int charidx = 0;
    int valueidx = 0;
    bool copy = false;
    while(client.connected()) {
      if(client.available() ){
        // read an incoming byte from the server and print it to serial monitor:
        char c = client.read();
        if(copy && c == '"'){
          copy = false;
        }
        else if(!copy && c == '"'){
          copy = true;
        }
        if(copy){
          if(c == ','){
            valueidx++;
            charidx = 0;
          }
          else{
            if(c != '"'){
              response[valueidx][charidx] = c;
              charidx++;
            }
          }
        }
      }
    }

    moistureMinimum = atoi(response[0]);
    lightMinimum = atoi(response[1]);
    automaticWatering = response[2][0] == 'T';
    automaticLight = response[3][0] == 'T';
    
    // the server's disconnected, stop the client:
    client.stop();
    Serial.println();
    Serial.println("disconnected");
  } else {// if not connected:
    Serial.println("connection failed");
  }
}

//sends the measured plant state to the webserver
void sendSensorValuesToServer(){
  String queryString = "moist=";
  queryString += moistureValue;
  queryString += "&level=";
  queryString += liquidLevelValue;
  queryString += "&light=";
  queryString += photoresistorValue;
  queryString += "&led=";
  queryString += ledBrightness;

  if(client.connect(HOST_NAME, HTTP_PORT)) {
    // if connected:
    Serial.println("Connected to server");
    // make a HTTP request:
    // send HTTP header
    client.println("POST /SetPlantState?"+ queryString +" HTTP/1.1");
    client.println("Host: " + String(HOST_NAME));
    client.println("Content-Type: application/json");
    client.println("Connection: close");
    client.print("Content-Length: ");
    client.println("0");
    client.println(); // end HTTP header

    // send HTTP body
    client.println();

    Serial.flush();

    // the server's disconnected, stop the client:
    client.stop();
    Serial.println();
    Serial.println("disconnected");
  } else {// if not connected:
    Serial.println("connection failed");
  }
  
}
