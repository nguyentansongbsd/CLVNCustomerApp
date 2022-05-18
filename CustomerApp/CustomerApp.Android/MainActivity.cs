using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Xamarin.Forms;
using CustomerApp.IServices;
using Android.Gms.Common;
using System.Threading.Tasks;
using Android.Util;
using Plugin.FirebasePushNotification;
using Android.Content;
using Newtonsoft.Json;
using System.Collections.Generic;
using Android.Support.V4.App;

namespace CustomerApp.Droid
{
    [Activity(Label = "BSD Customer", Icon = "@drawable/Logo", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Stormlion.PhotoBrowser.Droid.Platform.Init(this);
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            InitNotification();
            LoadApplication(new App());
            
            DependencyService.Get<ILoadingService>().Initilize();

            FirebasePushNotificationManager.ProcessIntent(this, Intent);
        }

        public void InitNotification()
        {
            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                FirebasePushNotificationManager.DefaultNotificationChannelId = "DefaultChannel";
                FirebasePushNotificationManager.DefaultNotificationChannelName = "General";
                FirebasePushNotificationManager.DefaultNotificationChannelImportance = NotificationImportance.Max;
            }

            FirebasePushNotificationManager.NotificationActivityFlags = ActivityFlags.SingleTop;
            FirebasePushNotificationManager.Initialize(this, false);
            CrossFirebasePushNotification.Current.OnNotificationReceived += Current_OnNotificationReceived;
        }

        private void Current_OnNotificationReceived(object source, FirebasePushNotificationDataEventArgs e)
        {
            if (e.Data.ContainsKey("NotificationData"))
            {
                string NotificationJson = e.Data["NotificationData"].ToString();
                SendLocalNotification(e.Data);
            }
        }

        private static int currentNotiicationId = 0;
        private void SendLocalNotification(IDictionary<string, object> data)
        {
            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.NewTask);

            foreach (var key in data.Keys) intent.PutExtra(key, data[key].ToString());
            PendingIntent pendingIntent = PendingIntent.GetActivity(this, 100, intent, PendingIntentFlags.UpdateCurrent);
            var notificationBuilder = new NotificationCompat.Builder(this, "DefaultChannel")
                                      .SetSmallIcon(Resource.Drawable.Logo)
                                      .SetContentTitle(data["title"].ToString())
                                      .SetContentText(data["body"].ToString())
                                      .SetAutoCancel(true)
                                      .SetContentIntent(pendingIntent);

            currentNotiicationId = currentNotiicationId > 3000 ? 0 : currentNotiicationId++;
            NotificationManagerCompat.From(this).Notify("", currentNotiicationId, notificationBuilder.Build());
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            FirebasePushNotificationManager.ProcessIntent(this, intent);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
