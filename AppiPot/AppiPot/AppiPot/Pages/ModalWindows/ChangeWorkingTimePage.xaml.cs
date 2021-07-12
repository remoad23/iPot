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
    public partial class ChangeWorkingTimePage : ContentPage
    {
        public ChangeWorkingTimePage()
        {
            InitializeComponent();
        }

        public async void SendWorkingTime(object sender, EventArgs args)
        {
            
            MessagingCenter.Send<App,string>(
                App.Current as App,
                "SendWorkingTime",
                "");
            var from = this.FindByName<TimePicker>("From");
            var to = this.FindByName<TimePicker>("To");
            SendInformation(from.Time.ToString(),to.Time.ToString());
            await Navigation.PopModalAsync();
        }

        public async void CancelWorkingTime(object sender, EventArgs args) => await Navigation.PopModalAsync();
        
        public async void SendInformation(string from,string to)
        {
            
            using (var client = new HttpClient(GetInsecureHandler()))
            {
                client.BaseAddress = new Uri("https://10.0.2.2:5001/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                StringContent content = new StringContent(JsonConvert.SerializeObject(new {uptimeStart = from,uptimeEnd = to}), Encoding.UTF8, "application/json");
                
                
                // HTTP POST
                HttpResponseMessage response = await client.PostAsync("SetUptime",content);
                
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