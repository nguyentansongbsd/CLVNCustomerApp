using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomerApp.Helper;
using CustomerApp.Models;
using CustomerApp.Resources;
using CustomerApp.Settings;
using CustomerApp.ViewModels;
using Newtonsoft.Json;
using Telerik.XamarinForms.Primitives;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CustomerApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private string _userName;
        public string UserName { get => _userName; set { _userName = value; OnPropertyChanged(nameof(UserName)); } }
        private string _password;
        public string Password { get => _password; set { _password = value; OnPropertyChanged(nameof(Password)); } }
        private bool _eyePass = false;
        public bool EyePass { get => _eyePass; set { _eyePass = value; OnPropertyChanged(nameof(EyePass)); } }
        private bool _issShowPass = true;
        public bool IsShowPass { get => _issShowPass; set { _issShowPass = value; OnPropertyChanged(nameof(IsShowPass)); } }

        private string _verApp;
        public string VerApp { get => _verApp; set { _verApp = value; OnPropertyChanged(nameof(VerApp)); } }

        public string ImeiNum { get; set; }
        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = this;

            VerApp = Config.OrgConfig.VerApp;
            if (UserLogged.IsLogged && UserLogged.IsSaveInforUser)
            {
                checkboxRememberAcc.IsChecked = true;
                UserName = UserLogged.User;
                Password = UserLogged.Password;
                SetGridUserName();
                SetGridPassword();
            }
            else
            {
                checkboxRememberAcc.IsChecked = false;
            }

            if (UserLogged.Language == "vi")
            {
                flagVN.BorderColor = Color.FromHex("#2196F3");
                flagEN.BorderColor = Color.FromHex("#eeeeee");
            }

            else if (UserLogged.Language == "en")
            {
                flagVN.BorderColor = Color.FromHex("#eeeeee");
                flagEN.BorderColor = Color.FromHex("#2196F3");
            }
        }

        protected override bool OnBackButtonPressed()
        {
            System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
            return true;
        }

        private void IsRemember_Tapped(object sender, EventArgs e)
        {
            checkboxRememberAcc.IsChecked = !checkboxRememberAcc.IsChecked;
        }

        private void UserName_Focused(object sender, EventArgs e)
        {
            entryUserName.Placeholder = "";
            SetGridUserName();
        }

        private void UserName_UnFocused(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UserName))
            {
                Grid.SetRow(lblUserName, 0);
                Grid.SetRow(entryUserName, 0);
                Grid.SetRowSpan(entryUserName, 2);

                entryUserName.Placeholder = Language.ten_dang_nhap;
            }
        }

        private void SetGridUserName()
        {
            Grid.SetRow(lblUserName, 0);
            Grid.SetRow(entryUserName, 1);
            Grid.SetRowSpan(entryUserName, 1);
        }

        private void Password_Focused(object sender, EventArgs e)
        {
            entryPassword.Placeholder = "";
            SetGridPassword();
            lblEyePass.Margin = new Thickness(0, 0, 0, 0);
        }

        private void Password_UnFocused(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Password))
            {
                Grid.SetRow(lblPassword, 1);
                Grid.SetRow(entryPassword, 1);
                Grid.SetRowSpan(entryPassword, 2);
                if (Device.RuntimePlatform == Device.iOS)
                {
                    lblEyePass.Margin = new Thickness(0, 0, 0, -13);
                }
                else
                {
                    lblEyePass.Margin = new Thickness(0, 0, 0, -10);
                }

                EyePass = false;
                entryPassword.Placeholder = Language.mat_khau;
            }
        }

        private void Password_TextChanged(object sender, EventArgs e)
        {
            if (!EyePass)
            {
                EyePass = string.IsNullOrWhiteSpace(Password) ? false : true;
            }
        }

        private void SetGridPassword()
        {
            Grid.SetRow(lblPassword, 0);
            Grid.SetRow(entryPassword, 1);
            Grid.SetRowSpan(entryPassword, 1);
        }

        private void ShowHidePass_Tapped(object sender, EventArgs e)
        {
            IsShowPass = !IsShowPass;
            if (IsShowPass)
            {
                lblEyePass.Text = "\uf070";
            }
            else
            {
                lblEyePass.Text = "\uf06e";
            }
        }

        private void Flag_Tapped(object sender, EventArgs e)
        {
            string code = (string)((sender as RadBorder).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            if (code == UserLogged.Language) return;
            LoadingHelper.Show();
            UserLogged.Language = code;
            CultureInfo cultureInfo = new CultureInfo(UserLogged.Language);
            Language.Culture = cultureInfo;
            if (code == "vi")
            {
                flagVN.BorderColor = Color.FromHex("#2196F3");
                flagEN.BorderColor = Color.FromHex("#eeeeee");
                CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("vi-VN");
            }

            else if (code == "en")
            {
                flagVN.BorderColor = Color.FromHex("#eeeeee");
                flagEN.BorderColor = Color.FromHex("#2196F3");
                CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
            }
            Application.Current.MainPage = new AppShell();
            Shell.Current.Navigation.PushAsync(new LoginPage(), false);
            LoadingHelper.Hide();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UserName))
            {
                ToastMessageHelper.ShortMessage(Language.ten_dang_nhap_khong_duoc_de_trong);
                return;
            }
            if (string.IsNullOrWhiteSpace(Password))
            {
                ToastMessageHelper.ShortMessage(Language.mat_khau_khong_duong_de_trong);
                return;
            }
            try
            {
                LoadingHelper.Show();
                await Task.Delay(1);
                var response = await LoginHelper.Login();
                if (response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    GetTokenResponse tokenData = JsonConvert.DeserializeObject<GetTokenResponse>(body);
                    UserLogged.AccessToken = tokenData.access_token;

                    UserModel user = await LoginUser();
                    if (user != null)
                    {
                        if (user.fullname != UserName && user.emailaddress1 != UserName)
                        {
                            LoadingHelper.Hide();
                            ToastMessageHelper.ShortMessage(Language.ten_dang_nhap_khong_dung);
                            return;
                        }

                        if (user.bsd_password != Password)
                        {
                            LoadingHelper.Hide();
                            ToastMessageHelper.ShortMessage(Language.mat_khau_khong_dung);
                            return;
                        }

                        UserLogged.Id = user.contactid;
                        UserLogged.User = user.fullname;
                        UserLogged.Email = user.emailaddress1;
                        UserLogged.Password = user.bsd_password;
                        UserLogged.ManagerId = user._ownerid_value;
                        UserLogged.IsSaveInforUser = checkboxRememberAcc.IsChecked;
                        UserLogged.IsLogged = true;
                        UserLogged.Avartar = user.entityimage;

                        Application.Current.MainPage = new AppShell();

                        LoadingHelper.Hide();
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage(Language.khong_tim_thay_user);
                    }
                }
            }
            catch (Exception ex)
            {
                LoadingHelper.Hide();
                ToastMessageHelper.LongMessage(ex.Message);
            }
        }

        public async Task<UserModel> LoginUser()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                  <entity name='contact'>
                    <attribute name='contactid' />
                    <attribute name='fullname' />
                    <attribute name='emailaddress1' />
                    <attribute name='ownerid'/>
                    <attribute name='bsd_password'/>
                    <attribute name='entityimage'/>
                    <order attribute='fullname' descending='false' />
                    <filter type='or'>
                      <condition attribute='fullname' operator='eq' value='{UserName}' />
                      <condition attribute='emailaddress1' operator='eq' value='{UserName}' />
                    </filter>
                  </entity>
                </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<UserModel>>("contacts", fetchXml);
            if (result == null || result.value.Count == 0)
            {
                return null;
            }
            else
            {
                return result.value.FirstOrDefault();
            }
        }

        private async void ForgotPassword_Tapped(object sender, EventArgs e)
        {
            try
            {
                LoadingHelper.Show();
                await Shell.Current.Navigation.PushAsync(new ForgotPassWordPage());
                LoadingHelper.Hide();
            }
            catch (Exception ex)
            { 
            
            }
        }
    }
}
