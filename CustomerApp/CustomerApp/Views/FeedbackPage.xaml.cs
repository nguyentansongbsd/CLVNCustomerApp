using CustomerApp.Helper;
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
                MailAddress from = new MailAddress("phupx@bsdinsight.com","Feedback-" + viewModel.Contact.bsd_fullname);
                MailAddress to = new MailAddress("songnt@bsdinsight.com", "Contact"); //songnt@bsdinsight.com
                MailMessage mail = new MailMessage()
                {
                    From = from,
                    Subject = viewModel.Subject,
                    Body = viewModel.Content
                };
                mail.To.Add(to);
                client.SendCompleted += Client_SendCompleted;
                await client.SendMailAsync(mail);
            }
        }

        private void Client_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if(e.Error != null)
            {
                ToastMessageHelper.ShortMessage("Gửi phản hồi thất bại. " + e.Error.Message);
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