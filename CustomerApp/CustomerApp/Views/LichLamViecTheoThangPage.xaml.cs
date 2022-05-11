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
    public partial class LichLamViecTheoThangPage : ContentPage
    {
        LichLamViecViewModel viewModel;
        public Action<bool> OnComplete;
        public static bool? NeedToRefresh = null;
        public static bool? NeedToRefreshContentMeeting = null;
        public LichLamViecTheoThangPage()
        {
            InitializeComponent();
            this.BindingContext = viewModel = new LichLamViecViewModel();
            NeedToRefresh = false;
            reset();
        }

        public void reset()
        {
            this.viewModel.reset();
            this.loadData();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (NeedToRefresh == true)
            {
                LoadingHelper.Show();
                await viewModel.LoadMeetings();
                NeedToRefresh = false;
                LoadingHelper.Hide();
            }
        }

        public async void loadData()
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
            viewModel.selectedDate = DateTime.Today;
            this.seletedDay(viewModel.selectedDate.Value);
            if (viewModel.lstEvents != null && viewModel.lstEvents.Count > 0)
                OnComplete?.Invoke(true);
            else
                OnComplete?.Invoke(false);
        }

        private void seletedDay(DateTime d)
        {
            viewModel.selectedDate = d;
            viewModel.UpdateSelectedEvents(d);
        }

        private void CloseContentActivity_Tapped(object sender, EventArgs e)
        {
            ContentActivity.IsVisible = false;
        }

        private void Handle_SelectionChanged(System.Object sender, Telerik.XamarinForms.Input.Calendar.CalendarSelectionChangedEventArgs<System.Object> e)
        {
            this.seletedDay((DateTime)e.NewValue);
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
