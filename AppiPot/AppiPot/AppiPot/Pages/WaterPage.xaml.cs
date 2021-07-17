using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AppiPot.Pages.ModalWindows;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppiPot.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WaterPage : ContentPage
    {
        public WaterPage()
        {
            InitializeComponent();
            
            MessagingCenter.Subscribe<App, Tuple<string,TimeSpan>>(
                App.Current as App,
                "AddPourTime",
                async (sender, arg) =>
            {
                var labelTime = new Label();
                labelTime.Text = arg.Item2.ToString();
                var labelPercentage = new Label();
                labelPercentage.Text = arg.Item1;
                var switcher = new Switch();
                var timeList = this.FindByName<Grid>("TimeList");
                
                int rowToPlace = timeList.Children.Count / 3 + 1;
                
                timeList.Children.Add(labelTime,2,rowToPlace);
                timeList.Children.Add(labelPercentage,4,rowToPlace);
                timeList.Children.Add(switcher,1,rowToPlace);
                SendWaterPourTime(Int32.Parse(arg.Item1),arg.Item2);
                // await DisplayAlert("Message received", "arg=" + arg, "OK");
            });
            
            GetWaterAndMoisture();
        }

        /**
         * Pour the Water by sending data to APi and then to arduino
         */
        public async void ChangeMinimalDryness(object sender, EventArgs args)
        {
            var addPourTimePage = new ChangeMinimalDrynessPage();
            await Navigation.PushModalAsync(addPourTimePage);
        }

        /**
         * Add new Time to pour plant with water
         */
        public async void PourWater(object sender, EventArgs args)
        {
            using (var client = new HttpClient(GetInsecureHandler()))
            {
                client.BaseAddress = App.Adress;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                StringContent content = new StringContent(JsonConvert.SerializeObject(new {NeedsWater = true}), Encoding.UTF8, "application/json");
                
                // HTTP POST
                HttpResponseMessage response = await client.PostAsync("SetNeedsWater",content);
            }

        }

        public async void GetWaterAndMoisture()
        {
            using (var client = new HttpClient(GetInsecureHandler()))
            {
                client.BaseAddress = App.Adress;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                HttpResponseMessage response = await client.GetAsync("GetMoistureAndWater");
                
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var waterAndMoisture = JsonConvert.DeserializeObject<Tuple<float,float>>(data);
                    
                    string waterState = waterAndMoisture.Item2 == 0 ? "Auffüllen" : "Genug";
                    this.FindByName<Label>("WaterState").Text = waterState;
                    var moistureState= waterAndMoisture.Item1 == 0.00 ? 0 : ( waterAndMoisture.Item1 / 1023) * 100; 
                    this.FindByName<Label>("MoistureState").Text = moistureState.ToString("0.00") + "%";
                }
            }
        }
                
        
        
        public async void SendWaterPourTime(int percentage,TimeSpan time)
        {
            
            using (var client = new HttpClient(GetInsecureHandler()))
            {
                client.BaseAddress = App.Adress;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                StringContent content = new StringContent(JsonConvert.SerializeObject(new {Percentage = percentage,Time = time.ToString()}), Encoding.UTF8, "application/json");
                
                // HTTP POST
                HttpResponseMessage response = await client.PostAsync("CreateTimeToPour",content);
                
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                 //   product = JsonConvert.DeserializeObject<UserAccount>(data);
                }
            }
            
        }
        
        // This method must be in a class in a platform project, even if
        // the HttpClient object is constructed in a shared project.
        public HttpClientHandler GetInsecureHandler()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                if (cert.Issuer.Equals("CN=localhost"))
                    return true;
                return errors == System.Net.Security.SslPolicyErrors.None;
            };
            return handler;
        }
        
    }
}