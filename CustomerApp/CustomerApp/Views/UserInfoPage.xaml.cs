using CustomerApp.Helper;
using CustomerApp.Resources;
using CustomerApp.Settings;
using CustomerApp.ViewModels;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CustomerApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserInfoPage : ContentPage
    {
        private UserInfoPageViewModel viewModel;
        public UserInfoPage()
        {
            InitializeComponent();
            Init();
        }
        private async void Init()
        {
            LoadingHelper.Show();
            this.BindingContext = viewModel = new UserInfoPageViewModel();
            centerModelPassword.Body.BindingContext = viewModel;
            await Task.WhenAll(
                viewModel.LoadContact(),
                viewModel.LoadLoyalty());
            LoadingHelper.Hide();
        }
        private async void Save_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(viewModel.Contact.mobilephone))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_nhap_so_dien_thoai);
                return;
            }

            if (viewModel.Contact.birthdate == null)
            {
                ToastMessageHelper.ShortMessage(Language.noti_vui_long_chon_ngay_sinh);
                return;
            }

            if (viewModel.Gender == null)
            {
                ToastMessageHelper.ShortMessage(Language.noti_vui_long_chon_gioi_tinh);
                return;
            }

            LoadingHelper.Show();

            bool isSuccess = await viewModel.UpdateUserInfor();
            if (isSuccess)
            {
                if (viewModel.Contact.bsd_fullname != UserLogged.ContactName)
                {
                    UserLogged.ContactName = viewModel.Contact.bsd_fullname;
                }
                if (AppShell.NeedToRefeshUserInfo.HasValue) AppShell.NeedToRefeshUserInfo = true;
                ToastMessageHelper.ShortMessage(Language.noti_cap_nhat_thanh_cong);
                LoadingHelper.Hide();
            }
            else
            {
                LoadingHelper.Hide();
                ToastMessageHelper.ShortMessage(Language.noti_cap_nhat_that_bai);
            }
        }
        private async void ChangePassword_Tapped(object sender, EventArgs e)
        {
            viewModel.OldPassword = null;
            viewModel.NewPassword = null;
            viewModel.ConfirmNewPassword = null;
            await centerModelPassword.Show();
        }
        private void OldPassword_TextChanged(object sender, EventArgs e)
        {
            if (viewModel.OldPassword != null && viewModel.OldPassword.Contains(" "))
            {
                ToastMessageHelper.ShortMessage(Language.noti_mat_khau_khong_duoc_chua_ky_tu_khoan_trang);
                viewModel.OldPassword = viewModel.OldPassword.Trim();
            }
        }

        private void NewPassword_TextChanged(object sender, EventArgs e)
        {
            if (viewModel.NewPassword != null && viewModel.NewPassword.Contains(" "))
            {
                ToastMessageHelper.ShortMessage(Language.noti_mat_khau_khong_duoc_chua_ky_tu_khoan_trang);
                viewModel.NewPassword = viewModel.NewPassword.Trim();
            }
        }

        private void ConfirmPassword_TextChanged(object sender, EventArgs e)
        {
            if (viewModel.ConfirmNewPassword != null && viewModel.ConfirmNewPassword.Contains(" "))
            {
                ToastMessageHelper.ShortMessage(Language.noti_mat_khau_khong_duoc_chua_ky_tu_khoan_trang);
                viewModel.ConfirmNewPassword = viewModel.ConfirmNewPassword.Trim();
            }
        }

        private async void SaveChangedPassword_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(UserLogged.Password) && string.IsNullOrWhiteSpace(viewModel.OldPassword))
            {
                ToastMessageHelper.ShortMessage(Language.noti_vui_long_nhap_mat_khau_cu);
                return;
            }

            if (string.IsNullOrWhiteSpace(viewModel.NewPassword))
            {
                ToastMessageHelper.ShortMessage(Language.noti_vui_long_nhap_mat_khau_moi);
                return;
            }
            if (string.IsNullOrWhiteSpace(viewModel.ConfirmNewPassword))
            {
                ToastMessageHelper.ShortMessage(Language.noti_vui_long_nhap_xac_nhan_mat_khau_moi);
                return;
            }

            if (!string.IsNullOrWhiteSpace(UserLogged.Password) && viewModel.OldPassword.Contains(" "))
            {
                ToastMessageHelper.ShortMessage(Language.noti_mat_khau_khong_duoc_chua_ky_tu_khoan_trang);
                return;
            }

            if (viewModel.NewPassword.Contains(" "))
            {
                ToastMessageHelper.ShortMessage(Language.noti_mat_khau_khong_duoc_chua_ky_tu_khoan_trang);
                return;
            }

            if (viewModel.ConfirmNewPassword.Contains(" "))
            {
                ToastMessageHelper.ShortMessage(Language.noti_mat_khau_khong_duoc_chua_ky_tu_khoan_trang);
                return;
            }

            if (viewModel.NewPassword.Length < 6)
            {
                ToastMessageHelper.ShortMessage(Language.mat_khau_it_nhat_6_ky_tu);
                return;
            }

            if (!string.IsNullOrWhiteSpace(UserLogged.Password) && UserLogged.Password != viewModel.OldPassword)
            {
                ToastMessageHelper.ShortMessage(Language.noti_mat_khau_cu_khong_dung);
                return;
            }

            if (viewModel.NewPassword != viewModel.ConfirmNewPassword)
            {
                ToastMessageHelper.ShortMessage(Language.noti_xac_nhan_mat_khau_khong_dung);
                return;
            }

            if (!string.IsNullOrWhiteSpace(UserLogged.Password) && viewModel.OldPassword == viewModel.NewPassword)
            {
                ToastMessageHelper.LongMessage(Language.noti_ban_dang_su_dung_mat_khau_cu_vui_long_nhap_lai);
                return;
            }

            LoadingHelper.Show();
            bool isSuccess = await viewModel.ChangePassword();
            if (isSuccess)
            {
                await centerModelPassword.Hide();
                UserLogged.Password = viewModel.ConfirmNewPassword;
                ToastMessageHelper.ShortMessage(Language.doi_mat_khau_thanh_cong);
                LoadingHelper.Hide();
            }
            else
            {
                LoadingHelper.Hide();
                ToastMessageHelper.ShortMessage(Language.noti_doi_mat_khau_that_bai);
            }
        }
        private async void ChangeAvatar_Tapped(object sender, EventArgs e)
        {
            string[] options = new string[] { Language.thu_vien, Language.chup_hinh };
            string asw = await DisplayActionSheet(Language.tuy_chon, Language.huy, null, options);
            if (asw == Language.thu_vien)
            {
                LoadingHelper.Show();
                await CrossMedia.Current.Initialize();
                PermissionStatus photostatus = await PermissionHelper.RequestPhotosPermission();
                if (photostatus == PermissionStatus.Granted)
                {
                    try
                    {
                        var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions() { PhotoSize = PhotoSize.MaxWidthHeight, MaxWidthHeight = 600 });
                        if (file != null)
                        {
                            viewModel.AvatarArr = System.IO.File.ReadAllBytes(file.Path);
                            string imgBase64 = Convert.ToBase64String(viewModel.AvatarArr);
                            viewModel.Avatar = imgBase64;
                            if (viewModel.Avatar != UserLogged.Avartar)
                            {
                                bool isSuccess = await viewModel.ChangeAvatar();
                                if (isSuccess)
                                {
                                    UserLogged.Avartar = viewModel.Avatar;
                                    if (AppShell.NeedToRefeshUserInfo.HasValue) AppShell.NeedToRefeshUserInfo = true;
                                    ToastMessageHelper.ShortMessage(Language.noti_doi_hinh_dai_dien_thanh_cong);
                                    LoadingHelper.Hide();
                                }
                                else
                                {
                                    LoadingHelper.Hide();
                                    ToastMessageHelper.ShortMessage(Language.noti_doi_hinh_dai_dien_that_bai);
                                }
                            }
                            LoadingHelper.Hide();
                        }
                    }
                    catch (Exception ex)
                    {
                        ToastMessageHelper.LongMessage(ex.Message);
                        LoadingHelper.Hide();
                    }
                }
                LoadingHelper.Hide();
            }
            else if (asw == Language.chup_hinh)
            {
                LoadingHelper.Show();
                await CrossMedia.Current.Initialize();
                PermissionStatus camerastatus = await PermissionHelper.RequestCameraPermission();
                if (camerastatus == PermissionStatus.Granted)
                {
                    string fileName = $"{Guid.NewGuid()}.jpg";
                    var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        Name = fileName,
                        SaveToAlbum = false,
                        PhotoSize = PhotoSize.MaxWidthHeight,
                        MaxWidthHeight = 600
                    });
                    if (file != null)
                    {
                        viewModel.AvatarArr = System.IO.File.ReadAllBytes(file.Path);
                        viewModel.Avatar = Convert.ToBase64String(viewModel.AvatarArr);
                        if (viewModel.Avatar != UserLogged.Avartar)
                        {
                            bool isSuccess = await viewModel.ChangeAvatar();
                            if (isSuccess)
                            {
                                UserLogged.Avartar = viewModel.Avatar;
                                if (AppShell.NeedToRefeshUserInfo.HasValue) AppShell.NeedToRefeshUserInfo = true;
                                ToastMessageHelper.ShortMessage(Language.noti_doi_hinh_dai_dien_thanh_cong);
                                LoadingHelper.Hide();
                            }
                            else
                            {
                                LoadingHelper.Hide();
                                ToastMessageHelper.ShortMessage(Language.noti_doi_hinh_dai_dien_that_bai);
                            }
                        }
                    }
                }
                LoadingHelper.Hide();
            }
        }
    }
}