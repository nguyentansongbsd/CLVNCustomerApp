﻿using CustomerApp.Helper;
using CustomerApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

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
        private async void SendEmail_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            if (string.IsNullOrWhiteSpace(viewModel.Subject))
            {
                ToastMessageHelper.ShortMessage("Vui lòng nhập subject");
                return;
            }    
                if (string.IsNullOrWhiteSpace(viewModel.Subject))
            {
                ToastMessageHelper.ShortMessage("Vui lòng nhập description");
                return;
            }
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
                    $"\n-Email: {viewModel.Contact.emailaddress1}" +
                    $"\n-Số điện thoại: {viewModel.Contact.mobilephone}" +
                    $"\n-Tiêu đề phản hồi: {viewModel.Subject}" +
                    $"\n-Nội dung:" +
                    $"\n {viewModel.Content}";
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
                ToastMessageHelper.ShortMessage("Gửi phản hồi thất bại. " + e.Error.Message);
                viewModel.Subject = null;
                viewModel.Content = null;
                LoadingHelper.Hide();
            }
            else
            {
                ToastMessageHelper.ShortMessage("Gửi phản hồi thành công");
                LoadingHelper.Hide();
            }
        }
    }
}