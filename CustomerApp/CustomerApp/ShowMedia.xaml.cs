using CustomerApp.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CustomerApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShowMedia : ContentPage
    {
        public Action<bool> OnCompleted;
        private string mediaSourceId { get; set; }
        private string folderId { get; set; }
        public ShowMedia(string FolderId, string MediaSourceId)
        {
            InitializeComponent();
            folderId = FolderId;
            mediaSourceId = MediaSourceId;
            Init();
        }

        private async void Init()
        {
            LoadingHelper.Show();
            if (videoView != null)
            {
                //var result = await CrmHelper.RetrieveImagesSharePoint<GrapDownLoadUrlModel>($"{folderId}/items/{mediaSourceId}/driveItem");
                //if (result != null)
                //{
                //    string url = result.MicrosoftGraphDownloadUrl;
                //    videoView.Source = url;
                //    OnCompleted?.Invoke(true);
                //}
                //else
                //{
                //    OnCompleted?.Invoke(false);
                //}
                await Task.Delay(1000);
                videoView.Source = mediaSourceId;
                OnCompleted?.Invoke(true);
            }
            else
                OnCompleted?.Invoke(false);
            LoadingHelper.Hide();
        }
        public void StopMedia()
        {
            if (videoView != null)
            {
                videoView.Stop();
            }
        }
    }
}