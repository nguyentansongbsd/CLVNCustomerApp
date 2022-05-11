using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Database;
using CustomerApp.Helper;
using CustomerApp.Models;
using CustomerApp.Resources;
using CustomerApp.ViewModels;
using Xamarin.Forms;

namespace CustomerApp.Views
{
    public partial class ForgotPassWordPage : ContentPage
    {
        public static bool? NeedRefreshForm = null;
        public ForgotPassWordPageViewModel viewModel;
        public ForgotPassWordPage()
        {
            InitializeComponent();
            this.BindingContext = viewModel = new ForgotPassWordPageViewModel();
            NeedRefreshForm = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (NeedRefreshForm == true)
            {
                stPhone.IsVisible = false;
                stChangePassWord.IsVisible = true;
            }
        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void ConfirmPhone_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(viewModel.UserName))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_nhap_ho_ten_email);
                return;
            }
            if (string.IsNullOrWhiteSpace(viewModel.Phone))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_nhap_so_dien_thoai);
                return;
            }

            LoadingHelper.Show();
            await viewModel.CheckUserName();

            if (viewModel.User == null)
            {
                ToastMessageHelper.ShortMessage(Language.user_name_khong_dung);
                LoadingHelper.Hide();
                return;
            }

            try
            {
                ConformOTPPage conformOTP = new ConformOTPPage(viewModel.Phone);
                conformOTP.OnCompeleted = async (isSuccess) => {
                    if (isSuccess)
                    {
                        await Navigation.PushAsync(conformOTP);
                        LoadingHelper.Hide();
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage(Language.da_co_loi_xay_ra_vui_long_thu_lai_sau);
                    }
                };
            }
            catch(FirebaseException ex)
            {
                LoadingHelper.Hide();
                ToastMessageHelper.LongMessage(ex.Message);
            }
        }

        private async void ConfirmChangedPassWord_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(viewModel.NewPassword))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_nhap_mat_khau);
                return;
            }
            if (viewModel.NewPassword.Length < 6)
            {
                ToastMessageHelper.ShortMessage(Language.mat_khau_it_nhat_6_ky_tu);
                return;
            }
            if (string.IsNullOrWhiteSpace(viewModel.ConfirmPassword))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_nhap_xac_nhan_mat_khau);
                return;
            }
            if (viewModel.ConfirmPassword != viewModel.NewPassword)
            {
                ToastMessageHelper.ShortMessage(Language.mat_khau_khong_khop);
                return;
            }

            LoadingHelper.Show();
            string path = $"/contacts({viewModel.User.contactid})";
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["bsd_password"] = viewModel.ConfirmPassword;

            var content = data as object;
            CrmApiResponse apiResponse = await CrmHelper.PatchData(path, content);
            if (apiResponse.IsSuccess)
            {
                ToastMessageHelper.ShortMessage(Language.doi_mat_khau_thanh_cong);
                await Navigation.PopAsync();
                LoadingHelper.Hide();
            }
            else
            {
                LoadingHelper.Hide();
                ToastMessageHelper.ShortMessage(Language.da_co_loi_xay_ra_vui_long_thu_lai_sau);
            }
        }
    }
}
