using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerApp.Datas;
using CustomerApp.Helper;
using CustomerApp.Models;
using CustomerApp.Resources;
using CustomerApp.ViewModels;
using Xamarin.Forms;

namespace CustomerApp.Views
{
    public partial class LichLamViecTheoTuanPage : ContentPage
    {
        public Action<bool> OnCompleted;
        public LichLamViecViewModel viewModel; 
        public LichLamViecTheoTuanPage()
        {
            InitializeComponent();
            this.BindingContext = viewModel = new LichLamViecViewModel();
            Init();
        }

        public async void Init()
        {
            await Task.WhenAll(
                viewModel.LoadMeetings(),
                viewModel.LoadQueues(),
                viewModel.LoadDeposits(),
                viewModel.LoadInstallments(),
                viewModel.LoadDepositsSigning(),
                viewModel.LoadDepositsSigning(),
                viewModel.LoadContracts()
                );
            if (viewModel.lstEvents != null && viewModel.lstEvents.Count > 0)
            {
                Handle_DateSelected(null, new Xamarin.Forms.DateChangedEventArgs(DateTime.Now, DateTime.Now));
                OnCompleted?.Invoke(true);
            }
            else
            {
                OnCompleted?.Invoke(false);
            }
        }

        void Handle_DateSelected(object sender, Xamarin.Forms.DateChangedEventArgs e)
        {
            viewModel.selectedDate = e.NewDate;
            viewModel.UpdateSelectedEventsForWeekView(viewModel.selectedDate.Value);
        }

        private async void Event_Tapped(object sender, ItemTappedEventArgs e)
        {
            LoadingHelper.Show();
            var val = e.Item as CalendarModel;
            if (val.Type == CalendarType.Meeting)
            {
                await Task.WhenAll(
                viewModel.loadMeet(val.Id),
                viewModel.loadFromToMeet(val.Id)
                );
                viewModel.ActivityStatusCode = StatusCodeActivityData.GetStatusCodeById(viewModel.Meet.statecode.ToString());
                viewModel.ActivityType = "Collection Meeting";
                if (viewModel.Meet.activityid != Guid.Empty)
                {
                    ContentActivity.IsVisible = true;
                    ContentMeet.IsVisible = true;
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.noti_khong_tim_thay_thong_tin_vui_long_thu_lai);
                }
            }
            else
            {
                LoadingHelper.Hide();
            }
        }

        private void CloseContentActivity_Tapped(object sender, EventArgs e)
        {
            ContentActivity.IsVisible = false;
        }

        private void Update_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            try
            {
                MeetingFormPage meetingForm = new MeetingFormPage(viewModel.Meet.activityid);
                meetingForm.OnCompleted = async (isSuccess) =>
                {
                    if (isSuccess)
                    {
                        await Navigation.PushAsync(meetingForm);
                        LoadingHelper.Hide();
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage(Language.noti_khong_tim_thay_thong_tin_vui_long_thu_lai);
                    }
                };
            }
            catch (Exception ex)
            {
                LoadingHelper.Hide();
                ToastMessageHelper.ShortMessage(ex.Message);
            }
        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            if (await viewModel.UpdateStatusMeet())
            {
                viewModel.ActivityStatusCode = StatusCodeActivityData.GetStatusCodeById(viewModel.Meet.statecode.ToString());
                LoadingHelper.Hide();
                ToastMessageHelper.ShortMessage("Cuộc họp đã được hủy");
            }
            else
            {
                LoadingHelper.Hide();
                ToastMessageHelper.ShortMessage("Lỗi khi hủy cuộc họp. Vui lòng thử lại");
            }
        }
    }
}
