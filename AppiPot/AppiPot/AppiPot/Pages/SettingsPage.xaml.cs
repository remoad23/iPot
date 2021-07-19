using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppiPot.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            ConfigureSettingsStartup();
        }

        public async void ToggleAutomaticWatering(object sender, ToggledEventArgs e)
        {
            using (var client = new HttpClient(GetInsecureHandler()))
            {
                client.BaseAddress = App.Adress;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                StringContent content = new StringContent(JsonConvert.SerializeObject(new {automaticWatering = e.Value}), Encoding.UTF8, "application/json");
                
                // HTTP POST
                HttpResponseMessage response = await client.PostAsync("SetAutomaticWatering",content);
                
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    //   product = JsonConvert.DeserializeObject<UserAccount>(data);
                }
            }
        }

        public async void ToggleAutomaticLight(object sender, ToggledEventArgs e)
        {
            using (var client = new HttpClient(GetInsecureHandler()))
            {
                client.BaseAddress = App.Adress;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                StringContent content = new StringContent(JsonConvert.SerializeObject(new {automaticLight = e.Value}), Encoding.UTF8, "application/json");
                
                // HTTP POST
                HttpResponseMessage response = await client.PostAsync("SetAutomaticLight",content);
                
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    //   product = JsonConvert.DeserializeObject<UserAccount>(data);
                }
            }
        }

        public async void ToggleNotificationWater(object sender, ToggledEventArgs e)
        {
            using (var client = new HttpClient(GetInsecureHandler()))
            {
                Console.WriteLine(e.Value);
                client.BaseAddress = App.Adress;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                StringContent content = new StringContent(JsonConvert.SerializeObject(new {NotificationWater = e.Value}), Encoding.UTF8, "application/json");
                
                // HTTP POST
                HttpResponseMessage response = await client.PostAsync("SetNotificationWater",content);
                
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    //   product = JsonConvert.DeserializeObject<UserAccount>(data);
                }
            }
        }

        public async void ConfigureSettingsStartup()
        {
            var automaticWatering =  this.FindByName<SwitchCell>("AutomaticWatering");
            var automaticLight =  this.FindByName<SwitchCell>("AutomaticLight");
            var notificationWater =  this.FindByName<SwitchCell>("NotificationWater");
            var notificationLight =  this.FindByName<SwitchCell>("NotificationLight");
            var notificationMoisture =  this.FindByName<SwitchCell>("NotificationMoisture");

            using (var client = new HttpClient(GetInsecureHandler()))
            {
                client.BaseAddress = App.Adress;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                HttpResponseMessage response = await client.GetAsync("GetSettings");
                
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var settingsConfiguration = JsonConvert.DeserializeObject<List<bool>>(data);
                    automaticWatering.On = settingsConfiguration[0];
                    automaticLight.On = settingsConfiguration[1];
                    notificationWater.On = settingsConfiguration[2];
                    notificationLight.On = settingsConfiguration[3];
                    notificationMoisture.On = settingsConfiguration[4];
                }
            }
        }
        
        
        public static async void GetSettingsForService()
        {


            using (var client = new HttpClient(GetInsecureHandlerStatic()))
            {
                client.BaseAddress = App.Adress;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                HttpResponseMessage response = await client.GetAsync("GetWaterNotificationAndWater");
                
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    bool needsWater = JsonConvert.DeserializeObject<bool>(data);
                    DependencyService.Get<INotification>().NeedsWatering = needsWater;
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
        
        
        // This method must be in a class in a platform project, even if
        // the HttpClient object is constructed in a shared project.
        public static HttpClientHandler GetInsecureHandlerStatic()
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