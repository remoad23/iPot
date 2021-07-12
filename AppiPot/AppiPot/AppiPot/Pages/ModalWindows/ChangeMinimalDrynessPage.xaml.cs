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

namespace AppiPot.Pages.ModalWindows
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangeMinimalDrynessPage : ContentPage
    {
        public ChangeMinimalDrynessPage()
        {
            InitializeComponent();
            
            
        }

        public async void SaveDryness(object sender, EventArgs args)
        {
            var dryness = this.FindByName<Slider>("DrynessSlider");
            SendInformation(dryness.Value);
            await Navigation.PopModalAsync();
        }

        public async void CancelDryness(object sender, EventArgs args) => await Navigation.PopModalAsync();
        
        public async void SendInformation(double value)
        {
            
            using (var client = new HttpClient(GetInsecureHandler()))
            {
                client.BaseAddress = new Uri("https://10.0.2.2:5001/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                StringContent content = new StringContent(JsonConvert.SerializeObject(new {MinimalMoisture = value}), Encoding.UTF8, "application/json");

                await DisplayAlert("Message received", value.ToString(), "OK");
                
                // HTTP POST
                HttpResponseMessage response = await client.PostAsync("SetMinimalMoisture",content);
                
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
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