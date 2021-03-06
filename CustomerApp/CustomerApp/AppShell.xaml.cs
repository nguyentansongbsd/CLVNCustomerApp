using System;
using System.Collections.Generic;
using CustomerApp.Helper;
using CustomerApp.Settings;
using CustomerApp.ViewModels;
using CustomerApp.Views;
using Xamarin.Forms;

namespace CustomerApp
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public static bool? NeedToRefeshUserInfo = null;
        private string _avartar;
        public string Avartar { get => _avartar; set { _avartar = value; OnPropertyChanged(nameof(Avartar)); } }

        private string _userName;
        public string UserName { get => _userName; set { _userName = value; OnPropertyChanged(nameof(UserName)); } }

        private string _contactName;
        public string ContactName { get => _contactName; set { _contactName = value; OnPropertyChanged(nameof(ContactName)); } }

        private string _verApp;
        public string VerApp { get => _verApp; set { _verApp = value; OnPropertyChanged(nameof(VerApp)); } }

        public AppShell()
        {
            InitializeComponent();
            UserName = UserLogged.User;
            ContactName = string.IsNullOrWhiteSpace(UserLogged.ContactName) ? UserLogged.User : UserLogged.ContactName;
            Avartar = UserLogged.Avartar;
            VerApp = Config.OrgConfig.VerApp;
            NeedToRefeshUserInfo = false;
            this.BindingContext = this;
        }

        protected override void OnNavigating(ShellNavigatingEventArgs args)
        {
            base.OnNavigating(args);
            if (NeedToRefeshUserInfo == true)
            {
                LoadingHelper.Show();
                if (Avartar != UserLogged.Avartar)
                {
                    Avartar = UserLogged.Avartar;
                }
                ContactName = UserLogged.ContactName;
                NeedToRefeshUserInfo = false;
                LoadingHelper.Hide();
            }
        }

        private async void UserInfor_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            if (UserLogged.Id == Guid.Empty)
            {
               // ToastMessageHelper.ShortMessage(Language.chua_co_contact_khong_the_chinh_sua_thong_tin);
                LoadingHelper.Hide();
                return;
            }

            await Shell.Current.Navigation.PushAsync(new UserInfoPage());
            this.FlyoutIsPresented = false;
            LoadingHelper.Hide();
        }

        private async void Logout_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}

