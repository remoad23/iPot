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
            GetMinimalMoisture();
        }
        
        protected override void OnAppearing()
        {
            GetMinimalMoisture();
        }
        
        public async void GetMinimalMoisture()
        {
            using (var client = new HttpClient(GetInsecureHandler()))
            {
                client.BaseAddress = App.Adress;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                HttpResponseMessage response = await client.GetAsync("GetMinimalMoisture");
                
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    float moisture = JsonConvert.DeserializeObject<float>(data);
                    this.FindByName<Slider>("DrynessSlider").Value = moisture;
                    moisture = moisture == 0 ? 0 : (moisture / 1023) * 100;
                    this.FindByName<Label>("SliderValueDryness").Text = moisture.ToString();
                }
            }
        }

        public async void SaveDryness(object sender, EventArgs args)
        {
            var dryness = this.FindByName<Slider>("DrynessSlider");
            SendInformation(dryness.Value);
            await Navigation.PopModalAsync();
        }

        public async void CancelDryness(object sender, EventArgs args) => await Navigation.PopModalAsync();
        
        public void ChangeSliderValue(object sender, EventArgs args)
        {
            float moisture = (float) ((Slider) sender).Value;
            moisture = moisture == 0 ? 0 : (moisture / 1023) * 100;
            this.FindByName<Label>("SliderValueDryness").Text = moisture.ToString();
        }
        
        public async void SendInformation(double value)
        {
            
            using (var client = new HttpClient(GetInsecureHandler()))
            {
                client.BaseAddress = App.Adress;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                StringContent content = new StringContent(JsonConvert.SerializeObject(new {MinimalMoisture = value}), Encoding.UTF8, "application/json");

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