using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CustomerApp.Views;
using System.Globalization;
using CustomerApp.Settings;
using CustomerApp.Resources;
using CustomerApp.IServices;
using Plugin.FirebasePushNotification;
using CustomerApp.Services;
using CustomerApp.Models;
using Newtonsoft.Json;
using CustomerApp.Helper;
using System.Collections.Generic;
using Firebase.Database;
using System.Threading.Tasks;
using Firebase.Database.Query;
using CustomerApp.ViewModels;

namespace CustomerApp
{
    public partial class App : Application
    {
        FirebaseClient firebaseClient = new FirebaseClient("https://smsappcrm-default-rtdb.asia-southeast1.firebasedatabase.app/",
            new FirebaseOptions { AuthTokenAsyncFactory = () => Task.FromResult("kLHIPuBhEIrL6s3J6NuHpQI13H7M0kHjBRLmGEPF") });
        bool test { get; set; }
        public App ()
        {
            InitializeComponent();
            CultureInfo cultureInfo = new CultureInfo(UserLogged.Language);
            Language.Culture = cultureInfo;
            MainPage = new LoginPage();
            //if (test == false)
            //{
            //    Shell.Current.Navigation.PushAsync(new LoginPage(), false);
            //}


            //MainPage = new BlankPage();
            DependencyService.Register<INotificationService, NotificationService>();
        }

        protected override void OnStart ()
        {
            CrossFirebasePushNotification.Current.OnTokenRefresh += async (s, p) =>
            {
                UserLogged.DeviceToken = await DependencyService.Get<INotificationService>().SaveToken();
                TokenModel data = new TokenModel();
                data.Token = UserLogged.DeviceToken;
                var a = firebaseClient.Child("NotificationToken").PutAsync(data);
            };
            CrossFirebasePushNotification.Current.OnNotificationOpened += Current_OnNotificationOpened;
        }

        private void Current_OnNotificationOpened(object source, FirebasePushNotificationResponseEventArgs p)
        {
            if (p.Data.ContainsKey("NotificationData"))
            {
                string NotificationJson = p.Data["NotificationData"].ToString();
                NotificaModel model = JsonConvert.DeserializeObject<NotificaModel>(NotificationJson);
                if (model.ProjectId != null)
                {
                    LoadingHelper.Show();
                    ProjectInfoPage project = new ProjectInfoPage(model.ProjectId, null, true);
                    project.OnCompleted = async (isSuccess) => {
                        if (isSuccess)
                        {
                            await Shell.Current.Navigation.PushAsync(project);
                            LoadingHelper.Hide();
                            NotificationPageViewModel notification = new NotificationPageViewModel();
                            await notification.UpdateStatus(model.Key, model);
                        }
                        else
                        {
                            LoadingHelper.Hide();
                            ToastMessageHelper.ShortMessage(Language.noti_khong_tim_thay_thong_tin_vui_long_thu_lai);
                        }
                    };
                }
            }
                

        }

        protected override void OnSleep ()
        {

        }

        protected override void OnResume ()
        {
        }
    }
}

