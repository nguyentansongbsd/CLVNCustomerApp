using CustomerApp.Datas;
using CustomerApp.Helper;
using CustomerApp.Models;
using CustomerApp.IServices;
using CustomerApp.Resources;
using CustomerApp.Settings;
using CustomerApp.ViewModels;
using Stormlion.PhotoBrowser;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CustomerApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProjectInfoPage : ContentPage
    {
        public Action<bool> OnCompleted;
        public static bool? NeedToRefreshQueue = null;
        public static bool? NeedToRefreshNumQueue = null;
        public ProjectInfoPageViewModel viewModel;
        private ShowMedia showMedia;
        FirebaseStorageHelper storageHelper = new FirebaseStorageHelper();
        private bool IsFromNotificationImg { get; set; }

        public ProjectInfoPage(Guid projectId, string projectName = null,bool isFromNotificationImg = false)
        {
            InitializeComponent();
            this.BindingContext = viewModel = new ProjectInfoPageViewModel();
            NeedToRefreshQueue = false;
            NeedToRefreshNumQueue = false;
            viewModel.ProjectId = projectId;
            viewModel.ProjectName = projectName;
            IsFromNotificationImg = isFromNotificationImg;
            Init();
        }

        public async void Init()
        {
            if (IsFromNotificationImg)
            {
                VisualStateManager.GoToState(radborderThongKe, "InActive");
                VisualStateManager.GoToState(radborderThongTin, "InActive");
                VisualStateManager.GoToState(radborderGiuCho, "Active");
                VisualStateManager.GoToState(lblThongKe, "InActive");
                VisualStateManager.GoToState(lblThongTin, "InActive");
                VisualStateManager.GoToState(lblGiuCho, "Active");
                VisualStateManager.GoToState(radborderPDF, "InActive");
                VisualStateManager.GoToState(lblPDF, "InActive");
                stackThongKe.IsVisible = false;
                stackThongTin.IsVisible = false;
                stackCollection.IsVisible = true;
                frAddFilePdf.IsVisible = stackPDF.IsVisible = false;
            }
            else
            {
                VisualStateManager.GoToState(radborderThongKe, "Active");
                VisualStateManager.GoToState(radborderThongTin, "InActive");
                VisualStateManager.GoToState(radborderGiuCho, "InActive");
                VisualStateManager.GoToState(lblThongKe, "Active");
                VisualStateManager.GoToState(lblThongTin, "InActive");
                VisualStateManager.GoToState(lblGiuCho, "InActive");
                VisualStateManager.GoToState(radborderPDF, "InActive");
                VisualStateManager.GoToState(lblPDF, "InActive");
                stackThongKe.IsVisible = true;
                stackThongTin.IsVisible = false;
                stackCollection.IsVisible = false;
                frAddFilePdf.IsVisible = stackPDF.IsVisible = false;
            }
            
            await Task.WhenAll(
                viewModel.LoadData(),
                viewModel.LoadAllCollection(),
                viewModel.CheckEvent(),
                viewModel.LoadThongKe(),
                viewModel.LoadThongKeGiuCho(),
                viewModel.LoadThongKeHopDong(),
                viewModel.LoadThongKeBangTinhGia(),
                viewModel.LoadPhasesLanch(),
                viewModel.LoadCollection()
            );

            if (viewModel.Project != null)
            {
                viewModel.ProjectType = Data.GetProjectTypeById(viewModel.Project.bsd_projecttype);
                viewModel.PropertyUsageType = Data.GetPropertyUsageTypeById(viewModel.Project.bsd_propertyusagetype.ToString());
                //if (viewModel.Project.bsd_handoverconditionminimum.HasValue)
                //{
                //    viewModel.HandoverCoditionMinimum = HandoverCoditionMinimumData.GetHandoverCoditionMinimum(viewModel.Project.bsd_handoverconditionminimum.Value.ToString());
                //}
                OnCompleted?.Invoke(true);
            }
            else
            {
                OnCompleted?.Invoke(false);
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (NeedToRefreshQueue == true)
            {
                LoadingHelper.Show();
                viewModel.PageListGiuCho = 1;
                viewModel.ListGiuCho.Clear();
                await viewModel.LoadGiuCho();
                NeedToRefreshQueue = false;
                LoadingHelper.Hide();
            }

            if (NeedToRefreshNumQueue == true)
            {
                LoadingHelper.Show();
                viewModel.SoGiuCho = 0;
                await viewModel.LoadThongKeGiuCho();
                NeedToRefreshNumQueue = false;
                LoadingHelper.Hide();
            }
            if (showMedia != null)
                showMedia.StopMedia();
        }

        private async void ThongKe_Tapped(object sender, EventArgs e)
        {
            VisualStateManager.GoToState(radborderThongKe, "Active");
            VisualStateManager.GoToState(radborderThongTin, "InActive");
            VisualStateManager.GoToState(radborderGiuCho, "InActive");
            VisualStateManager.GoToState(radborderPDF, "InActive");
            VisualStateManager.GoToState(lblThongKe, "Active");
            VisualStateManager.GoToState(lblThongTin, "InActive");
            VisualStateManager.GoToState(lblGiuCho, "InActive");
            VisualStateManager.GoToState(lblPDF, "InActive");
            stackThongKe.IsVisible = true;
            stackThongTin.IsVisible = false;
            stackGiuCho.IsVisible = false;
            stackCollection.IsVisible = false;
            frAddFilePdf.IsVisible = stackPDF.IsVisible = false;
        }

        private async void ThongTin_Tapped(object sender, EventArgs e)
        {
            VisualStateManager.GoToState(radborderThongKe, "InActive");
            VisualStateManager.GoToState(radborderThongTin, "Active");
            VisualStateManager.GoToState(radborderGiuCho, "InActive");
            VisualStateManager.GoToState(radborderPDF, "InActive");
            VisualStateManager.GoToState(lblThongKe, "InActive");
            VisualStateManager.GoToState(lblThongTin, "Active");
            VisualStateManager.GoToState(lblGiuCho, "InActive");
            VisualStateManager.GoToState(lblPDF, "InActive");
            stackThongKe.IsVisible = false;
            stackThongTin.IsVisible = true;
            stackGiuCho.IsVisible = false;
            frAddFilePdf.IsVisible = stackPDF.IsVisible = false;
            stackCollection.IsVisible = false;
        }

        private async void GiuCho_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            VisualStateManager.GoToState(radborderThongKe, "InActive");
            VisualStateManager.GoToState(radborderThongTin, "InActive");
            VisualStateManager.GoToState(radborderPDF, "InActive");
            VisualStateManager.GoToState(radborderGiuCho, "Active");
            VisualStateManager.GoToState(lblThongKe, "InActive");
            VisualStateManager.GoToState(lblThongTin, "InActive");
            VisualStateManager.GoToState(lblGiuCho, "Active");
            VisualStateManager.GoToState(lblPDF, "InActive");
            stackThongKe.IsVisible = false;
            stackThongTin.IsVisible = false;
           // stackGiuCho.IsVisible = true;
            frAddFilePdf.IsVisible = stackPDF.IsVisible = false;
            stackCollection.IsVisible = true;
            //if (viewModel.IsLoadedGiuCho == false)
            //{
            //    await viewModel.LoadGiuCho();
            //}
            if (viewModel.ListCollection == null)
            {
                await viewModel.LoadCollection();
            }
            LoadingHelper.Hide();
        }

        private async void PDF_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            VisualStateManager.GoToState(radborderThongKe, "InActive");
            VisualStateManager.GoToState(radborderThongTin, "InActive");
            VisualStateManager.GoToState(radborderGiuCho, "InActive");
            VisualStateManager.GoToState(radborderPDF, "Active");
            VisualStateManager.GoToState(lblThongKe, "InActive");
            VisualStateManager.GoToState(lblThongTin, "InActive");
            VisualStateManager.GoToState(lblGiuCho, "InActive");
            VisualStateManager.GoToState(lblPDF, "Active");
            stackThongKe.IsVisible = false;
            stackThongTin.IsVisible = false;
//             stackGiuCho.IsVisible = false;
            stackCollection.IsVisible = false;
            frAddFilePdf.IsVisible = stackPDF.IsVisible = true;
            if (viewModel.ListPDF.Count == 0)
            {
                await viewModel.LoadPDF();
            }
            LoadingHelper.Hide();
        }

        private void GiuCho_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            QueueForm queue = new QueueForm(viewModel.ProjectId, false);
            queue.OnCompleted = async (IsSuccess) => {
                if (IsSuccess)
                {
                    await Navigation.PushAsync(queue);
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    //ToastMessageHelper.ShortMessage(Language.khong_tim_thay_san_pham);
                }
            };
        }

        private async void ShowMoreListDatCho_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PageListGiuCho++;
            await viewModel.LoadGiuCho();
            LoadingHelper.Hide();
        }

        private void ChuDauTu_Tapped(System.Object sender, System.EventArgs e)
        {
            //LoadingHelper.Show();
            //var id = (Guid)((sender as Label).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            //AccountDetailPage accountDetailPage = new AccountDetailPage(id);
            //accountDetailPage.OnCompleted = async (IsSuccess) => {
            //    if (IsSuccess)
            //    {
            //        await Navigation.PushAsync(accountDetailPage);
            //        LoadingHelper.Hide();
            //    }
            //    else
            //    {
            //        LoadingHelper.Hide();
            //        ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
            //    }
            //};
        }

        private void GiuChoItem_Tapped(object sender, EventArgs e)
        {
            //LoadingHelper.Show();
            //var itemId = (Guid)((sender as Grid).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            //QueuesDetialPage queuesDetialPage = new QueuesDetialPage(itemId);
            //queuesDetialPage.OnCompleted = async (IsSuccess) => {
            //    if (IsSuccess)
            //    {
            //        await Navigation.PushAsync(queuesDetialPage);
            //        LoadingHelper.Hide();
            //    }
            //    else
            //    {
            //        LoadingHelper.Hide();
            //        ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
            //    }
            //};
        }

        private void ItemSlider_Tapped(object sender, EventArgs e)
        {
            // khoa lai vi phu long chua co hinh anh va video

            //LoadingHelper.Show();
            //var item = (CollectionData)((sender as Grid).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            //if (item.SharePointType == SharePointType.Image)
            //{
            //    var img = viewModel.Photos.SingleOrDefault(x => x.URL == item.ImageSource);
            //    var index = viewModel.Photos.IndexOf(img);

            //    new PhotoBrowser()
            //    {
            //        Photos = viewModel.Photos,
            //        StartIndex = index,
            //        EnableGrid = true
            //    }.Show();
            //}
            //else if (item.SharePointType == SharePointType.Video)
            //{
            //    ShowMedia showMedia = new ShowMedia(Config.OrgConfig.SharePointProjectId, item.MediaSourceId);
            //    showMedia.OnCompleted = async (isSuccess) => {
            //        if (isSuccess)
            //        {
            //            await Navigation.PushAsync(showMedia);
            //            LoadingHelper.Hide();
            //        }
            //        else
            //        {
            //            LoadingHelper.Hide();
            //            ToastMessageHelper.ShortMessage("Không lấy được video");
            //        }
            //    };
            //}
            //LoadingHelper.Hide();
        }

        private void ScollTo_Video_Tapped(object sender, EventArgs e)
        {
            var index = viewModel.ListCollection.IndexOf(viewModel.ListCollection.FirstOrDefault(x => x.SharePointType == SharePointType.Video));
            carouseView.ScrollTo(index, position: ScrollToPosition.End);
        }

        private void ScollTo_Image_Tapped(object sender, EventArgs e)
        {
            var index = viewModel.ListCollection.IndexOf(viewModel.ListCollection.FirstOrDefault(x => x.SharePointType == SharePointType.Image));
            carouseView.ScrollTo(index, position: ScrollToPosition.End);
        }

        private async void OpenEvent_Tapped(object sender, EventArgs e)
        {
            if (viewModel.Event == null)
            {
                await viewModel.LoadDataEvent();
            }
            ContentEvent.IsVisible = true;
        }

        private void CloseContentEvent_Tapped(object sender, EventArgs e)
        {
            ContentEvent.IsVisible = false;
        }

        private void PhasesLanch_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            var itemId = (Guid)((sender as Grid).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            PhasesLanchDetailPage phasesLanchDetailPage = new PhasesLanchDetailPage(itemId);
            phasesLanchDetailPage.OnCompleted = async (IsSuccess) =>
            {
                if (IsSuccess)
                {
                    await Navigation.PushAsync(phasesLanchDetailPage);
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.noti_khong_tim_thay_thong_tin_vui_long_thu_lai);
                }
            };
        }

        private void ListCollection_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            var item = (CollectionData)((sender as Grid).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            if (item.SharePointType == SharePointType.Image)
            {
                var img = viewModel.Photos.SingleOrDefault(x => x.URL == item.ImageSource);
                var index = viewModel.Photos.IndexOf(img);

                new PhotoBrowser()
                {
                    Photos = viewModel.Photos,
                    StartIndex = index,
                    EnableGrid = true
                }.Show();
            }
            else if (item.SharePointType == SharePointType.Video)
            {
                showMedia = new ShowMedia(item.MediaSourceId, item.MediaSourceId);
                showMedia.OnCompleted = async (isSuccess) =>
                {
                    if (isSuccess)
                    {
                        await Navigation.PushAsync(showMedia);
                        LoadingHelper.Hide();
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage("Không lấy được video");
                    }
                };
            }
            LoadingHelper.Hide();
        }

        private async void File_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            string fileName = (string)((sender as Grid).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            string file = await storageHelper.GetFile(fileName);
            try
            {
                await DependencyService.Get<IPdfService>().View(file, "File Pdf");
            }
            catch(Exception ex)
            {

            }
            
            LoadingHelper.Hide();
        }

        private async void AddFilePdf_Clicked(object sender, EventArgs e)
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.StorageRead>();
                }
                if (status != PermissionStatus.Granted) return;

                var pickerFile = await FilePicker.PickAsync(new PickOptions()
                {
                    FileTypes = FilePickerFileType.Pdf,
                    PickerTitle = "Chọn File",
                });
                if (pickerFile != null)
                {
                    LoadingHelper.Show();
                    viewModel.ListPDF.Add(new Models.PDFModel() { id = Guid.NewGuid(), name = pickerFile.FileName });
                    viewModel.Stream = await pickerFile.OpenReadAsync();

                    var isSuccess = await storageHelper.UploadFile(viewModel.Stream,viewModel.Project.bsd_projectcode,"pdf", pickerFile.FileName);
                    if (!string.IsNullOrWhiteSpace(isSuccess))
                    {
                        UserLogged.ListPdf = JsonConvert.SerializeObject(viewModel.ListPDF);
                        ToastMessageHelper.ShortMessage(Language.noti_thanh_cong);
                    }
                    LoadingHelper.Hide();
                }
                else
                {

                }
            }
            catch(Exception ex)
            {

            }
        }
    }
}