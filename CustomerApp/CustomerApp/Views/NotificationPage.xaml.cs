using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerApp.Helper;
using CustomerApp.Models;
using CustomerApp.Resources;
using CustomerApp.ViewModels;
using Xamarin.Forms;

namespace CustomerApp.Views
{
    public partial class NotificationPage : ContentPage
    {
        public NotificationPageViewModel viewModel;
        public NotificationPage()
        {
            InitializeComponent();
            this.BindingContext = viewModel = new NotificationPageViewModel();
            Init();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
        }

        private async void Init()
        {
            await viewModel.LoadData();
        }

        private async void IsReadAllNoti_Tapped(object sender, EventArgs e)
        {
            var accept = await DisplayAlert("", "Bạn có muốn đánh dấu tất cả thông báo là đã đọc không?", Language.dong_y,Language.huy);
            if (accept)
            {
                LoadingHelper.Show();
                foreach (var item in viewModel.Notifications)
                {
                    if (item.IsBusy == false)
                    {
                        await viewModel.UpdateStatus(item.Key, item);
                    }
                }
                if (DashboardPage.NeedToRefesh.HasValue) DashboardPage.NeedToRefesh = true;
                viewModel.Notifications.Clear();
                await viewModel.LoadData();
                LoadingHelper.Hide();
            }
        }

        private async void ListView_ItemTapped(System.Object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            var item = e.Item as NotificaModel;
            if (item.ProjectId != null)
            {
                LoadingHelper.Show();
                ProjectInfoPage project = new ProjectInfoPage(item.ProjectId, null, true);
                project.OnCompleted = async (isSuccess) => {
                    if (isSuccess)
                    {
                        await Navigation.PushAsync(project);
                        LoadingHelper.Hide();
                        if (item.IsRead == true) return;
                        await viewModel.UpdateStatus(item.Key, item);
                        if (DashboardPage.NeedToRefesh.HasValue) DashboardPage.NeedToRefesh = true;
                        viewModel.Notifications.Clear();
                        await viewModel.LoadData();
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
}
