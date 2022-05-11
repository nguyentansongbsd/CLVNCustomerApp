using System;
using Foundation;
using UIKit;
using CustomerApp.IServices;
using CustomerApp.iOS.Services;

[assembly: Xamarin.Forms.Dependency(typeof(OpenAppSettings))]
namespace CustomerApp.iOS.Services
{
    public class OpenAppSettings : IOpenAppSettings
    {
        public void Open()
        {
            UIApplication.SharedApplication.OpenUrl(new NSUrl("app-settings:"));
        }
    }
}
