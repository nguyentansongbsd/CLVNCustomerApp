using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerApp.Helper;
using CustomerApp.Models;
using CustomerApp.Resources;
using CustomerApp.ViewModels;
using Firebase.Database;
using Xamarin.Forms;

namespace CustomerApp.Views
{
    public partial class DashboardPage : ContentPage
    {
        FirebaseClient firebaseClient = new FirebaseClient("https://smsappcrm-default-rtdb.asia-southeast1.firebasedatabase.app/",
            new FirebaseOptions { AuthTokenAsyncFactory = () => Task.FromResult("kLHIPuBhEIrL6s3J6NuHpQI13H7M0kHjBRLmGEPF") });

        public DashboardPageViewModel viewModel;
        public static bool? NeedToRefesh = null;
        public DashboardPage()
        {
            InitializeComponent();
            this.BindingContext = viewModel = new DashboardPageViewModel();
            NeedToRefesh = false;
            Init();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (NeedToRefesh == true)
            {
                var GetItems = (await firebaseClient
                                  .Child("Notifications")
                                  .OnceAsync<NotificaModel>()).Select(item => new NotificaModel
                                  {
                                      Key = item.Key,
                                      Id = item.Object.Id,
                                      Title = item.Object.Title,
                                      Body = item.Object.Body,
                                      IsRead = item.Object.IsRead,
                                      CreatedDate = item.Object.CreatedDate,
                                  });
                viewModel.NumNoti = 0;
                foreach (var item in GetItems)
                {
                    if (item.IsRead == false)
                    {
                        viewModel.NumNoti++;
                    }
                }
            }
            
        }

        public async void Init()
        {
            await Task.WhenAll( viewModel.LoadContracts(),viewModel.LoadProjects());
            
            try
            {

                var collection = firebaseClient
                    .Child("Notifications")
                    .AsObservable<NotificaModel>()
                    .Subscribe(async (dbevent) =>
                    {
                        if (dbevent.EventType != Firebase.Database.Streaming.FirebaseEventType.Delete)
                        {
                            if (dbevent.Object.IsRead == false)
                            {
                                viewModel.NumNoti++;
                            }
                        }
                        else if (dbevent.EventType == Firebase.Database.Streaming.FirebaseEventType.Delete)
                        {
                        }
                    });
            }
            catch(Exception ex)
            {

            }

            //await Task.WhenAll(
            //    viewModel.LoadContactLoyalty(),
            //    viewModel.LoadQueueFourMonths(),
            //    viewModel.LoadQuoteFourMonths(),
            //    viewModel.LoadOptionEntryFourMonths(),
            //    viewModel.LoadUnitFourMonths(),
            //    viewModel.LoadMeetings(),
            //    viewModel.LoadQueues(),
            //    viewModel.LoadDeposits(),
            //    viewModel.LoadInstallments(),
            //    viewModel.LoadDepositsSigning(),
            //    viewModel.LoadDepositsSigning(),
            //    viewModel.LoadContracts()
            //    ) ;
        }

        private async void GoToNotificationPage_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NotificationPage());
        }


        private void ContractDetail_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            try
            {
                Guid id = (Guid)((sender as Grid).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
                ContractDetailPage contract = new ContractDetailPage(id);
                contract.OnCompleted = async (isSuccess) => {
                    if (isSuccess)
                    {
                        await Navigation.PushAsync(contract);
                        LoadingHelper.Hide();
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage(Language.noti_khong_tim_thay_thong_tin_vui_long_thu_lai);
                    }
                };

            }
            catch(Exception ex)
            {

            }
            
        }
        
        private async void ShowMore_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            await Navigation.PushAsync(new TransactionPage(true));
            LoadingHelper.Hide();
        }

        
        private async void ShowMoreDuAn_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            await Navigation.PushAsync(new ProjectsPage());
            LoadingHelper.Hide();
        }

        private void ProjectDetail_Tapped(object sender, EventArgs e)
        {
            try
            {
                LoadingHelper.Show();
                string id = (string)((sender as Grid).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
                ProjectInfoPage project = new ProjectInfoPage(Guid.Parse(id));
                project.OnCompleted = async (isSuccess) =>
                {
                    if (isSuccess)
                    {
                        await Navigation.PushAsync(project);
                        LoadingHelper.Hide();
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage(Language.noti_khong_tim_thay_thong_tin_vui_long_thu_lai);
                    }
                };
            }
            catch(Exception ex)
            {

            }
            
        }

    }
}
