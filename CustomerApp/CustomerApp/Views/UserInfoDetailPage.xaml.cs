using CustomerApp.Helper;
using CustomerApp.Models;
using CustomerApp.Resources;
using CustomerApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CustomerApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserInfoDetailPage : ContentPage
    {
        private UserInfoDetailPageViewModel viewModel;
        public Action<bool> OnCompleted;
        public UserInfoDetailPage()
        {
            InitializeComponent();
            Init();
        }
        public async void Init()
        {
            LoadingHelper.Show();
            this.BindingContext = viewModel = new UserInfoDetailPageViewModel();
            Tab_Tapped(1);
            await viewModel.LoadContact();
            if (viewModel.Contact.contactid != Guid.Empty)
                OnCompleted?.Invoke(true);
            else
                OnCompleted?.Invoke(false);
            LoadingHelper.Hide();
        }
        private void LoadDataPhongThuy()
        {
            if (viewModel.PhongThuy == null)
            {
                viewModel.PhongThuy = new PhongThuyModel();
                viewModel.LoadPhongThuy();
            }
        }
        private async void LoadDataLoyalty()
        {
            if (viewModel.Loyalty == null)
                await viewModel.LoadLoyalty();
        }
        private void ThongTin_Tapped(object sender, EventArgs e)
        {
            Tab_Tapped(1);
        }
        private void Loyalty_Tapped(object sender, EventArgs e)
        {
            Tab_Tapped(2);
            LoadDataLoyalty();
        }
        private void PhongThuy_Tapped(object sender, EventArgs e)
        {
            Tab_Tapped(3);
            LoadDataPhongThuy();
        }
        private void Tab_Tapped(int tab)
        {
            //if (tab == 1)
            //{
            //    VisualStateManager.GoToState(radBorderThongTin, "Selected");
            //    VisualStateManager.GoToState(lbThongTin, "Selected");
            //    TabThongTin.IsVisible = true;
            //}
            //else
            //{
            //    VisualStateManager.GoToState(radBorderThongTin, "Normal");
            //    VisualStateManager.GoToState(lbThongTin, "Normal");
            //    TabThongTin.IsVisible = false;
            //}
            //if (tab == 2)
            //{
            //    VisualStateManager.GoToState(radBorderLoyalty, "Selected");
            //    VisualStateManager.GoToState(lbLoyalty, "Selected");
            //    TabLoyalty.IsVisible = true;
            //}
            //else
            //{
            //    VisualStateManager.GoToState(radBorderLoyalty, "Normal");
            //    VisualStateManager.GoToState(lbLoyalty, "Normal");
            //    TabLoyalty.IsVisible = false;
            //}
            //if (tab == 3)
            //{
            //    VisualStateManager.GoToState(radBorderPhongThuy, "Selected");
            //    VisualStateManager.GoToState(lbPhongThuy, "Selected");
            //    TabPhongThuy.IsVisible = true;
            //}
            //else
            //{
            //    VisualStateManager.GoToState(radBorderPhongThuy, "Normal");
            //    VisualStateManager.GoToState(lbPhongThuy, "Normal");
            //    TabPhongThuy.IsVisible = false;
            //}
        }

        private void UpdateEmail_Phone_Clicked(object sender, EventArgs e)
        {
            UpdateEmail_Phone.IsVisible = true;
        }

        private void CloseContentUpdateEmail_Phone_Tapped(object sender, EventArgs e)
        {
            UpdateEmail_Phone.IsVisible = false;
        }

        private async void ChangeEmailPhone_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(viewModel.Email) && string.IsNullOrWhiteSpace(viewModel.Phone))
            {
                ToastMessageHelper.ShortMessage("Vui lòng nhập email / số điện thoại");
                return;
            }

            if (viewModel.Email == viewModel.Contact.emailaddress1)
            {
                ToastMessageHelper.ShortMessage("Bạn đang nhập email cũ");
                return;
            }

            if (viewModel.Phone == viewModel.Contact.mobilephone)
            {
                ToastMessageHelper.ShortMessage("Bạn đang nhập số điện thoại cũ");
                return;
            }

            LoadingHelper.Show();
            bool IsSuccess = await viewModel.CreateTask();
            if (IsSuccess)
            {
                ToastMessageHelper.ShortMessage(Language.noti_thanh_cong);
                UpdateEmail_Phone.IsVisible = false;
                viewModel.Email = null;
                viewModel.Phone = null;
                LoadingHelper.Hide();
            }
            else
            {
                LoadingHelper.Hide();
                ToastMessageHelper.ShortMessage(Language.noti_that_bai);
            }
        }
    }
}