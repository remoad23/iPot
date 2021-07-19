using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Arch.Lifecycle;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Util;
using AppiPot.Android;
using AppiPot.Pages;
using Newtonsoft.Json;
using Xamarin.Forms;

[assembly: Dependency(typeof(NotificationService))]

namespace AppiPot.Android
{
    public class NotificationService : INotification
    {
        public bool NeedsWatering { get; set; }
        
        
        public void StartForegroundServiceCompat()
        {
            var intent = new Intent(MainActivity.activity, typeof(myLocationService));

         //   MainActivity.activity.StartForegroundService(intent);
            
            MainActivity.activity.StartService(intent);
        }
    }

    [Service(Name = "com.xamarin.myLocationService",
        Process=":myLocationService_process",
        Exported=true)]
    public class myLocationService : Service
    {
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            Notification.Builder notificationBuilder = new Notification.Builder(this)
                .SetContentTitle("Alarm")
                .SetContentText("message")
                .SetAutoCancel(true);

            Observe();
            
            //Write want you want to do here
            return StartCommandResult.Sticky;
        }

        private async void Observe()
        {
            while (true)
            {
                SettingsPage.GetSettingsForService();
                await Task.Delay(2000);
                
                if (DependencyService.Get<INotification>().NeedsWatering)
                {
                    SendNotification();
                } 
            }
        }

        private void SendNotification()
        {
            CreateNotificationChannel();
            PublishNotification();
        }
        
        private void CreateNotificationChannel()
        {
            
            var channel = new NotificationChannel("CHANNEL_ID", "Resource.String.channel_name", NotificationImportance.Default)
            {
                Description = "Resource.String.channel_description"
            };

            var notificationManager = (NotificationManager) GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }

        private void PublishNotification()
        {
            // Instantiate the builder and set notification elements:
            NotificationCompat.Builder builder = new NotificationCompat.Builder(this, "CHANNEL_ID")
                .SetContentTitle("Wasserstand niedrig")
                .SetContentText("Deine Pflanze benötigt Wasser!")
                .SetSmallIcon (Resource.Drawable.mr_dialog_close_light);

            // Build the notification:
            Notification notification = builder.Build();

            // Get the notification manager:
            NotificationManager notificationManager =
                GetSystemService(Context.NotificationService) as NotificationManager;

            // Publish the notification:
            notificationManager.Notify(0, notification);
        }
    }

}