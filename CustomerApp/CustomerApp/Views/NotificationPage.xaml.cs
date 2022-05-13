using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerApp.Helper;
using CustomerApp.Models;
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

        private async void Init()
        {
            await viewModel.LoadData();
        }

        private async void IsReadAllNoti_Tapped(object sender, EventArgs e)
        {
            var accept = await DisplayAlert("", "Bạn có muốn đánh dấu tất cả thông báo là đã đọc không?", "Yes","No");
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
            LoadingHelper.Show();
            var item = e.Item as NotificaModel;
            if (item.IsRead == true) return;
            item.IsRead = true;
            await viewModel.UpdateStatus(item.Key, item);
            if (DashboardPage.NeedToRefesh.HasValue) DashboardPage.NeedToRefesh = true;
            viewModel.Notifications.Clear();
            await viewModel.LoadData();
            LoadingHelper.Hide();
        }

        
    }
}
