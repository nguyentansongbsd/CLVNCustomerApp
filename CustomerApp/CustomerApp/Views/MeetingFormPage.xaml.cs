using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerApp.Helper;
using CustomerApp.Resources;
using CustomerApp.ViewModels;
using Xamarin.Forms;

namespace CustomerApp.Views
{
    public partial class MeetingFormPage : ContentPage
    {
        public Action<bool> OnCompleted;
        public MeetingFormPageViewModel viewModel;
        private bool IsInit;

        public MeetingFormPage(Guid meetingId)
        {
            InitializeComponent();
            this.BindingContext = viewModel = new MeetingFormPageViewModel();
            viewModel.MeetingId = meetingId;
            Init();
        }

        public async void Init()
        {
            await Task.WhenAll(
                viewModel.loadDataMeet(),
                viewModel.LoadRequiredAndOptional()
                );
            if (viewModel.MeetingModel != null)
            {
                OnCompleted?.Invoke(true);
            }
            else
            {
                OnCompleted?.Invoke(false);
            }
        }

        private void DatePickerStart_DateSelected(object sender, EventArgs e)
        {
            if (IsInit)
            {
                if (viewModel.MeetingModel.scheduledend != null)
                {
                    if (this.compareDateTime(viewModel.MeetingModel.scheduledstart, viewModel.MeetingModel.scheduledend) != -1)
                    {
                        ToastMessageHelper.ShortMessage(Language.vui_long_chon_thoi_gian_ket_thuc_lon_hon_thoi_gian_bat_dau);
                        viewModel.MeetingModel.scheduledstart = viewModel.MeetingModel.scheduledend;
                    }
                }
            }
        }

        private void DatePickerEnd_DateSelected(object sender, EventArgs e)
        {
            if (IsInit)
            {
                if (viewModel.MeetingModel.scheduledstart != null)
                {
                    if (this.compareDateTime(viewModel.MeetingModel.scheduledstart, viewModel.MeetingModel.scheduledend) != -1)
                    {
                        ToastMessageHelper.ShortMessage(Language.vui_long_chon_thoi_gian_ket_thuc_lon_hon_thoi_gian_bat_dau);
                        viewModel.MeetingModel.scheduledend = viewModel.MeetingModel.scheduledstart;
                    }
                }
                else
                {
                    ToastMessageHelper.ShortMessage(Language.vui_long_chon_thoi_gian_bat_dau);
                }
            }
        }

        private int compareDateTime(DateTime? date, DateTime? date1)
        {
            if (date != null && date1 != null)
            {
                DateTime timeStart = new DateTime(date.Value.Year, date.Value.Month, date.Value.Day, date.Value.Hour, date.Value.Minute, 0);
                DateTime timeEnd = new DateTime(date1.Value.Year, date1.Value.Month, date1.Value.Day, date1.Value.Hour, date1.Value.Minute, 0);
                int result = DateTime.Compare(timeStart, timeEnd);
                if (result < 0)
                    return -1;
                else if (result == 0)
                    return 0;
                else
                    return 1;
            }
            if (date == null && date1 != null)
                return -1;
            if (date1 == null && date != null)
                return 1;
            return 0;
        }

        private void AllDayEvent_changeChecked(object sender, EventArgs e)
        {
            if (viewModel.MeetingModel.scheduledstart != null)
            {
                if (viewModel.MeetingModel.isalldayevent)
                {
                    var timeStart = viewModel.MeetingModel.scheduledstart.Value;
                    viewModel.MeetingModel.timeStart = new TimeSpan(timeStart.Hour, timeStart.Minute, timeStart.Second);
                    if (viewModel.MeetingModel.scheduledend != null)
                    {
                        var actualdurationminutes = Math.Round((viewModel.MeetingModel.scheduledend.Value - viewModel.MeetingModel.scheduledstart.Value).TotalMinutes);
                        viewModel.MeetingModel.scheduleddurationminutes = int.Parse(actualdurationminutes.ToString());
                    }
                    else
                    {
                        viewModel.MeetingModel.scheduleddurationminutes = 0;
                    }

                    viewModel.MeetingModel.scheduledstart = new DateTime(timeStart.Year, timeStart.Month, timeStart.Day, 7, 0, 0);
                    viewModel.MeetingModel.scheduledend = viewModel.MeetingModel.scheduledstart.Value.AddDays(1);
                }
                else
                {
                    var dateStart = viewModel.MeetingModel.scheduledstart.Value;
                    TimeSpan timeStart = viewModel.MeetingModel.timeStart;

                    if (viewModel.MeetingModel.timeStart != new TimeSpan(0, 0, 0))
                        viewModel.MeetingModel.scheduledstart = new DateTime(dateStart.Year, dateStart.Month, dateStart.Day, timeStart.Hours, timeStart.Minutes, timeStart.Seconds);
                    else
                        viewModel.MeetingModel.scheduledstart = new DateTime(dateStart.Year, dateStart.Month, dateStart.Day, dateStart.Hour, dateStart.Minute, dateStart.Second);

                    if (viewModel.MeetingModel.scheduleddurationminutes > 0)
                        viewModel.MeetingModel.scheduledend = viewModel.MeetingModel.scheduledstart.Value.AddMinutes(viewModel.MeetingModel.scheduleddurationminutes);
                    else
                        viewModel.MeetingModel.scheduledend = viewModel.MeetingModel.scheduledstart.Value.AddMinutes(1);
                }
            }
            else
            {
                viewModel.MeetingModel.isalldayevent = false;
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_thoi_gian_bat_dau);
            }
        }

        private async void Update_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            bool isUpdateSuccess = await viewModel.UpdateMeeting();
            if (isUpdateSuccess)
            {
                if (LichLamViecTheoThangPage.NeedToRefresh.HasValue) LichLamViecTheoThangPage.NeedToRefresh = true;
                ToastMessageHelper.ShortMessage(Language.noti_cap_nhat_thanh_cong);
                await Navigation.PopAsync();
                LoadingHelper.Hide();
            }
            else
            {
                LoadingHelper.Hide();
                ToastMessageHelper.ShortMessage(Language.noti_cap_nhat_that_bai) ;
            }
        }
    }
}
