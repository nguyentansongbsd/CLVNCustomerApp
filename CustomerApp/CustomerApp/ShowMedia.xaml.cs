using CustomerApp.Helper;
using CustomerApp.Resources;
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

        public async void Init()
        {
            videoView.Source = mediaSourceId;
            
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
                await Task.Delay(1);
                OnCompleted?.Invoke(true);
            }
            else
            {
                OnCompleted?.Invoke(false);
            }
                
        }

        protected override bool OnBackButtonPressed()
        {
            if (videoView.CanSeek == false)
            {
                ToastMessageHelper.ShortMessage(Language.dang_tai_video_vui_long_doi);
                return true;
            }
            LoadingHelper.Hide();
            return base.OnBackButtonPressed();
        }

        public void StopMedia()
        {
            try
            {
                if (videoView != null)
                {
                    videoView.Stop();
                }
            }
            catch (Exception ex)
            {

            }
            
        }

        void videoView_MediaOpened(System.Object sender, System.EventArgs e)
        {
            LoadingHelper.Hide();
        }
    }
}