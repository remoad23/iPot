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
    public partial class ChangeLightIntensityPage : ContentPage
    {
        public ChangeLightIntensityPage()
        {
            InitializeComponent();
            GetLight();
        }
        
        public async void GetLight()
        {
            using (var client = new HttpClient(GetInsecureHandler()))
            {
                client.BaseAddress = App.Adress;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                
                // HTTP POST
                HttpResponseMessage response = await client.GetAsync("GetMinimumLight");
                
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    float lightAndAmbient = JsonConvert.DeserializeObject<float>(data);
                    this.FindByName<Slider>("LightIntensitySlider2").Value =  lightAndAmbient;
                    lightAndAmbient = lightAndAmbient == 0 ? 0 : (lightAndAmbient / 1023) * 100;
                    this.FindByName<Label>("SliderValueLight").Text =  lightAndAmbient.ToString();
                }
            }
        }

        public async void SendLightIntensity(object sender, EventArgs args)
        {
            
            MessagingCenter.Send<App,string>(
                App.Current as App,
                "SendLightIntensity",
                "");

            var slider = this.FindByName<Slider>("LightIntensitySlider2");
            SendInformation(slider.Value);
            await Navigation.PopModalAsync();
        }

        public void ChangeSliderValue(object sender, EventArgs args)
        {
            float lightAndAmbient = (float)((Slider) sender).Value;
            lightAndAmbient = lightAndAmbient == 0 ? 0 : (lightAndAmbient / 1023) * 100;
            this.FindByName<Label>("SliderValueLight").Text = lightAndAmbient.ToString();
        }

        public async void CancelLightIntensity(object sender, EventArgs args) => await Navigation.PopModalAsync();
        
        public async void SendInformation(double lightIntensity)
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