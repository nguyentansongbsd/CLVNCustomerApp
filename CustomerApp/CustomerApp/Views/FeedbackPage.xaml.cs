using CustomerApp.Helper;
using CustomerApp.Resources;
using CustomerApp.ViewModels;
using System;
using System.Net;
using System.Net.Mail;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CustomerApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FeedbackPage : ContentPage
    {
        private FeedbackPageViewModel viewModel;
        public FeedbackPage()
        {
            InitializeComponent();
            Init();
        }

        private async void Init()
        {
            LoadingHelper.Show();
            this.BindingContext = viewModel = new FeedbackPageViewModel();
            await viewModel.LoadContact();
            LoadingHelper.Hide();
        }

        private void Phone_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (e.NewTextValue == null) return;
            if (e.NewTextValue != null && e.NewTextValue.Contains(",") || e.NewTextValue.Contains("."))
            {
                viewModel.Contact.mobilephone = e.NewTextValue.Replace(",", "").Replace(".", "");
            }
        }

        private async void SendEmail_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(viewModel.Contact.emailaddress1) || string.IsNullOrWhiteSpace(viewModel.Contact.mobilephone))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_nhap_email_hoac_so_dien_thoai);
                return;
            }

            if (!string.IsNullOrWhiteSpace(viewModel.Contact.emailaddress1) && !Helpers.Validations.IsValidEmail(viewModel.Contact.emailaddress1))
            {
                ToastMessageHelper.ShortMessage(Language.email_sai_dinh_dang);
                return;
            }

            if (viewModel.Contact.mobilephone.Length != 10)
            {
                ToastMessageHelper.ShortMessage(Language.so_dien_thoai_khong_hop_le);
                return;
            }

            if (string.IsNullOrWhiteSpace(viewModel.Subject))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_nhap_tieu_de);
                return;
            }
            if (string.IsNullOrWhiteSpace(viewModel.Content))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_nhap_mo_ta);
                return;
            }
            LoadingHelper.Show();
            if (viewModel.Contact != null)
            {
                SmtpClient client = new SmtpClient()
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential()
                    {
                        UserName = "phupx@bsdinsight.com",
                        Password = "kgakcewdhsqgxsfm"
                    }
                };
                MailAddress from = new MailAddress("phupx@bsdinsight.com", "BSD");
                MailAddress to = new MailAddress("hanph@bsdinsight.com", "CSKH Customer BSD"); //songnt@bsdinsight.com
                string contentEmail = $"Phản hồi từ khách hàng {viewModel.Contact.bsd_fullname}" +
                    $"\n- Email: {viewModel.Contact.emailaddress1}" +
                    $"\n- Số điện thoại: {viewModel.Contact.mobilephone}" +
                    $"\n- Tiêu đề phản hồi: {viewModel.Subject}\n" +
                    $"- Nội dung: " +
                    $"\n{viewModel.Content}";
                MailMessage mail = new MailMessage()
                {
                    From = from,
                    Subject = "Feedback Customer App - " + viewModel.Contact.bsd_fullname,
                    Body = contentEmail
                };
                mail.To.Add(to);
                client.SendCompleted += Client_SendCompleted;
                await client.SendMailAsync(mail);
            }
        }

        private void Client_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                ToastMessageHelper.ShortMessage(Language.noti_gui_phan_hoi_that_bai + e.Error.Message);
                LoadingHelper.Hide();
            }
            else
            {
                ToastMessageHelper.ShortMessage(Language.noti_gui_phan_hoi_thanh_cong);
                viewModel.Subject = null;
                viewModel.Content = null;
                LoadingHelper.Hide();
            }
        }
    }
}