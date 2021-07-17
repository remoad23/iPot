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
    public partial class LightPage : ContentPage
    {
        public LightPage()
        {
            InitializeComponent();
            GetLightAndAmbient();
            
            
            MessagingCenter.Subscribe<App,string>(
                App.Current as App,
                "SendWorkingTime",
                async (sender, arg) =>
                {
                });
            
            MessagingCenter.Subscribe<App,string>(
                App.Current as App,
                "SendLightIntensity",
                async (sender, arg) =>
                {
                    
                });
        }

        public async void ChangeLightIntensity(object sender, EventArgs e)
        {
            var addPourTimePage = new ChangeLightIntensityPage();
            await Navigation.PushModalAsync(addPourTimePage);
        }

        public async void ChangeWorkingTime(object sender, EventArgs e)
        {
            var addPourTimePage = new ChangeWorkingTimePage();
            await Navigation.PushModalAsync(addPourTimePage);
        }

        public void SetLightIntensityTo0(object sender, EventArgs e)
        {
            SendMinimumInformation(0);
        }
        
        public void SetLightIntensityTo25(object sender, EventArgs e)
        {
            SendMinimumInformation(25);
        }
        
        public void SetLightIntensityTo50(object sender, EventArgs e)
        {
            SendMinimumInformation(50);
        }
        
        public void SetLightIntensityTo75(object sender, EventArgs e)
        {
            SendMinimumInformation(75);
        }
        
        public void SetLightIntensityTo100(object sender, EventArgs e)
        {
            SendMinimumInformation(100);
        }
        
        public async void SendMinimumInformation(double lightIntensity)
        {
            
            using (var client = new HttpClient(GetInsecureHandler()))
            {
                client.BaseAddress = App.Adress;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                StringContent content = new StringContent(JsonConvert.SerializeObject(new {intensity = lightIntensity}), Encoding.UTF8, "application/json");

                // HTTP POST
                HttpResponseMessage response = await client.PostAsync("SetLightMinimum",content);
                
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                }
            }
            
        }

        public async void SendInformation(double lightIntensity)
        {
            
            using (var client = new HttpClient(GetInsecureHandler()))
            {
                client.BaseAddress = App.Adress;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                StringContent content = new StringContent(JsonConvert.SerializeObject(new {intensity = lightIntensity}), Encoding.UTF8, "application/json");

                // HTTP POST
                HttpResponseMessage response = await client.PostAsync("SetLightIntensity",content);
                
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                }
            }
            
        }
        
        public async void GetLightAndAmbient()
        {
            using (var client = new HttpClient(GetInsecureHandler()))
            {
                client.BaseAddress = App.Adress;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                
                // HTTP POST
                HttpResponseMessage response = await client.GetAsync("GetLightAndAmbient");
                
                if (response.IsSuccessStatusCode)
                {
                    // lightintensity 1023
                    // led intensity 0 - 255
                    var data = await response.Content.ReadAsStringAsync();
                    var lightAndAmbient = JsonConvert.DeserializeObject<Tuple<float,float>>(data);
                    float ledintensity  = lightAndAmbient.Item1 == 0 ? 0 : (lightAndAmbient.Item1 / 255) * 100;
                    float ambientIntensity = lightAndAmbient.Item2 == 0 ? 0 : (lightAndAmbient.Item2 / 1023) * 100;
                    
                    this.FindByName<Label>("lightState").Text =  ledintensity.ToString("0.00") + "%";
                    this.FindByName<Label>("RoomState").Text = ambientIntensity.ToString("0.00") + "%";
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