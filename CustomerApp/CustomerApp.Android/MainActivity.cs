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
            LoadApplication(new App());
            
            DependencyService.Get<ILoadingService>().Initilize();

            FirebasePushNotificationManager.ProcessIntent(this, Intent);
            FirebasePushNotificationManager.DefaultNotificationChannelId = "CLVNNotification";
            FirebasePushNotificationManager.DefaultNotificationChannelName = "CLVN";
            FirebasePushNotificationManager.DefaultNotificationChannelImportance = NotificationImportance.Max;

            FirebasePushNotificationManager.Initialize(this, true);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
