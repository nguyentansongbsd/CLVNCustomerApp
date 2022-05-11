using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CustomerApp.Views;
using System.Globalization;
using CustomerApp.Settings;
using CustomerApp.Resources;

namespace CustomerApp
{
    public partial class App : Application
    {
        public App ()
        {
            InitializeComponent();
            CultureInfo cultureInfo = new CultureInfo(UserLogged.Language);
            Language.Culture = cultureInfo;
            MainPage = new AppShell();
            Shell.Current.Navigation.PushAsync(new LoginPage(), false);
        }

        protected override void OnStart ()
        {
        }

        protected override void OnSleep ()
        {
        }

        protected override void OnResume ()
        {
        }
    }
}

