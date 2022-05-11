using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CustomerApp.Controls;
using CustomerApp.Helper;
using CustomerApp.Models;
using CustomerApp.Settings;
using Xamarin.Forms;

namespace CustomerApp.ViewModels
{
    public class LichLamViecViewModel : BaseViewModel
    {
        public ObservableCollection<CalendarModel> lstEvents { get; set; }
        private ObservableCollection<CalendarModel> _selectedDateEvents;
        public ObservableCollection<CalendarModel> selectedDateEvents { get => _selectedDateEvents; set { _selectedDateEvents = value; OnPropertyChanged(nameof(selectedDateEvents)); } }
        private ObservableCollection<Grouping<DateTime, CalendarModel>> _selectedDateEventsGrouped;
        public ObservableCollection<Grouping<DateTime, CalendarModel>> selectedDateEventsGrouped { get => _selectedDateEventsGrouped; set { _selectedDateEventsGrouped = value; OnPropertyChanged(nameof(selectedDateEventsGrouped)); } }

        public MeetingModel _meet;
        public MeetingModel Meet { get => _meet; set { _meet = value; OnPropertyChanged(nameof(Meet)); } }

        public bool _showGridButton;
        public bool ShowGridButton { get => _showGridButton; set { _showGridButton = value; OnPropertyChanged(nameof(ShowGridButton)); } }

        private StatusCodeModel _activityStatusCode;
        public StatusCodeModel ActivityStatusCode { get => _activityStatusCode; set { _activityStatusCode = value; OnPropertyChanged(nameof(ActivityStatusCode)); } }

        private string _activityType;
        public string ActivityType { get => _activityType; set { _activityType = value; OnPropertyChanged(nameof(ActivityType)); } }

        private DateTime? _scheduledStartTask;
        public DateTime? ScheduledStartTask { get => _scheduledStartTask; set { _scheduledStartTask = value; OnPropertyChanged(nameof(ScheduledStartTask)); } }

        private DateTime? _scheduledEndTask;
        public DateTime? ScheduledEndTask { get => _scheduledEndTask; set { _scheduledEndTask = value; OnPropertyChanged(nameof(ScheduledEndTask)); } }

        private DateTime? _selectedDate;
        public DateTime? selectedDate { get => _selectedDate; set { _selectedDate = value; DayLabel = ""; OnPropertyChanged(nameof(selectedDate)); } }

        public string CodeCompleted = "completed";
        public string CodeCancel = "cancel";

        private string _DayLabel;
        public string DayLabel
        {
            get
            {
                return _DayLabel;
            }
            set
            {
                if (this.selectedDate.HasValue)
                {
                    var date = selectedDate.Value;
                    var result = string.Format("{0}, Ngày {1} {2}", CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(date.DayOfWeek), date.Day, CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(date.Month));

                    if (Device.RuntimePlatform == Device.Android)
                    {
                        result = result.ToUpper();
                    }

                    _DayLabel = result;
                }
                OnPropertyChanged(nameof(DayLabel));
            }
        }

        public string entity { get; set; }
        public string EntityName { get; set; }

        public LichLamViecViewModel()
        {
            Meet = new MeetingModel();

            lstEvents = new ObservableCollection<CalendarModel>();
            selectedDateEvents = new ObservableCollection<CalendarModel>();
            selectedDateEventsGrouped = new ObservableCollection<Grouping<DateTime, CalendarModel>>();

            selectedDate = DateTime.Today;
        }

        public void reset()
        {
            lstEvents.Clear();
            selectedDate = null;
            selectedDateEvents.Clear();
            selectedDateEventsGrouped.Clear();
        }

        public async Task LoadMeetings()
        {
            string fetch = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='appointment'>
                                    <attribute name='subject' alias='Title'/>
                                    <attribute name='statecode' alias='State'/>
                                    <attribute name='activityid' alias='Id'/>
                                    <attribute name='scheduledstart' alias='StartDate'/>
                                    <attribute name='scheduledend' alias='EndDate' />
                                    <order attribute='modifiedon' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='isregularactivity' operator='eq' value='1' />
                                      <condition attribute='bsd_customer' operator='eq' value='{UserLogged.Id}' />
                                    </filter>
                              </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<CalendarModel>>("appointments", fetch);
            var data = result.value;
            if (data.Any())
            {
                foreach (var item in data)
                {
                    item.Type = CalendarType.Meeting;
                    item.StartDate = item.StartDate.ToLocalTime();
                    item.EndDate = item.EndDate.ToLocalTime();
                    if (item.State == "0")
                    {
                        item.Color = Color.FromHex("#8eff82"); //Green
                        item.BackgroundColor = Color.White;
                    }
                    else
                    {
                        item.Color = Color.FromHex("#d3ffce"); //Light Green
                        item.BackgroundColor = Color.FromHex("#d3ffce");
                    }
                    lstEvents.Add(item);
                }
            }
        }

        public async Task LoadQueues()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='opportunity'>
                                    <attribute name='name' alias='Title'/>
                                    <attribute name='statuscode' alias='State'/>
                                    <attribute name='opportunityid' alias='Id'/>
                                    <attribute name='bsd_queuingexpired' alias='EndDate'/>
                                    <attribute name='bsd_bookingtime' alias='StartDate'/>
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='bsd_prioritynumber' operator='null' />
                                      <condition attribute='bsd_priorityqueue' operator='null' />
                                      <condition attribute='bsd_queuingexpired' operator='not-null' />
                                      <condition attribute='bsd_bookingtime' operator='not-null' />
                                      <condition attribute='statuscode' operator='not-in'>
                                        <value>4</value>
                                        <value>100000003</value>
                                      </condition>
                                      <condition attribute='parentcontactid' operator='eq' value='{UserLogged.Id}'/>
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<CalendarModel>>("opportunities", fetchXml);
            var data = result.value;
            if (data.Any())
            {
                foreach (var item in data)
                {
                    item.Type = CalendarType.Booking;
                    item.Color = Color.FromHex("#54C483");
                    item.BackgroundColor = Color.FromHex("#54C483");
                    item.StartDate = item.StartDate.ToLocalTime();
                    item.EndDate = item.EndDate.ToLocalTime();
                    this.lstEvents.Add(item);
                }
            }
        }

        public async Task LoadDeposits()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='quote'>
                                    <attribute name='name' alias='Title'/>
                                    <attribute name='statuscode' alias='State'/>
                                    <attribute name='quoteid' alias='ID'/>
                                    <attribute name='createdon' alias='StartDate'/>
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='statuscode' operator='eq' value='861450002' />
                                      <condition attribute='statuscode' operator='ne' value='6' />
                                    </filter>
                                    <link-entity name='contactquotes' from='quoteid' to='quoteid' visible='false' intersect='true'>
                                      <link-entity name='contact' from='contactid' to='contactid' alias='aa'>
                                        <filter type='and'>
                                          <condition attribute='contactid' operator='eq' value='{UserLogged.Id}'/>
                                        </filter>
                                      </link-entity>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<CalendarModel>>("quotes", fetchXml);
            var data = result.value;
            if (data.Any())
            {
                foreach (var item in data)
                {
                    item.Type = CalendarType.Deposit;
                    item.StartDate = item.StartDate.ToLocalTime();
                    item.EndDate = item.StartDate.ToLocalTime();
                    item.Color = Color.FromHex("#FA7901");
                    item.BackgroundColor = Color.FromHex("#FA7901");
                    lstEvents.Add(item);
                }
            }
        }

        public async Task LoadInstallments()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_paymentschemedetail'>
                                    <attribute name='bsd_paymentschemedetailid' alias='Id'/>
                                    <attribute name='bsd_name' alias='Title' />
                                    <attribute name='createdon' />
                                    <attribute name='bsd_duedate' alias='StartDate'/>
                                    <attribute name='statuscode' alias='State'/>
                                    <order attribute='bsd_name' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='statuscode' operator='eq' value='100000000' />
                                    </filter>
                                    <link-entity name='salesorder' from='salesorderid' to='bsd_optionentry' link-type='inner' alias='ah'>
                                      <link-entity name='contactorders' from='salesorderid' to='salesorderid' visible='false' intersect='true'>
                                        <link-entity name='contact' from='contactid' to='contactid' alias='ai'>
                                          <filter type='and'>
                                            <condition attribute='contactid' operator='eq' value='{UserLogged.Id}'/>
                                          </filter>
                                        </link-entity>
                                      </link-entity>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<CalendarModel>>("bsd_paymentschemedetails", fetchXml);
            var data = result.value;
            if (data.Any())
            {
                foreach (var item in data)
                {
                    item.Type = CalendarType.Installment;
                    item.StartDate = item.StartDate.ToLocalTime();
                    item.EndDate = item.StartDate.ToLocalTime();
                    item.Color = Color.FromHex("#f79364");
                    item.BackgroundColor = Color.FromHex("#f79364");
                    lstEvents.Add(item);
                }
            }
        }

        public async Task LoadDepositsSigning()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='quote'>
                                    <attribute name='name' alias='Title'/>
                                    <attribute name='statuscode' alias='State'/>
                                    <attribute name='quoteid' alias='ID'/>
                                    <attribute name='createdon' alias='StartDate'/>
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='statuscode' operator='eq' value='861450002' />
                                      <condition attribute='statuscode' operator='ne' value='6' />
                                      <condition attribute='bsd_quotationprinteddate' operator='not-null' />
                                      <condition attribute='bsd_quotationsigneddate' operator='null' />
                                    </filter>
                                    <link-entity name='contactquotes' from='quoteid' to='quoteid' visible='false' intersect='true'>
                                      <link-entity name='contact' from='contactid' to='contactid' alias='aa'>
                                        <filter type='and'>
                                          <condition attribute='contactid' operator='eq' value='{UserLogged.Id}'/>
                                        </filter>
                                      </link-entity>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<CalendarModel>>("quotes", fetchXml);
            var data = result.value;
            if (data.Any())
            {
                foreach (var item in data)
                {
                    item.Type = CalendarType.Deposit;
                    item.StartDate = item.StartDate.ToLocalTime();
                    item.EndDate = item.StartDate.ToLocalTime();
                    item.Color = Color.FromHex("#FA7901");
                    item.BackgroundColor = Color.FromHex("#FA7901");
                    lstEvents.Add(item);
                }
            }
        }

        public async Task LoadDepositAgreement()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='salesorder'>
                                    <attribute name='name' alias='Title'/>
                                    <attribute name='statuscode' alias='State'/>
                                    <attribute name='createdon' alias='StartDate'/>
                                    <attribute name='salesorderid' alias='Id'/>
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='bsd_agreementdate' operator='not-null' />
                                      <condition attribute='bsd_signeddadate' operator='null' />
                                    </filter>
                                    <link-entity name='contactorders' from='salesorderid' to='salesorderid' visible='false' intersect='true'>
                                      <link-entity name='contact' from='contactid' to='contactid' alias='aa'>
                                        <filter type='and'>
                                          <condition attribute='contactid' operator='eq' value='{UserLogged.Id}' />
                                        </filter>
                                      </link-entity>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<CalendarModel>>("salesorders", fetchXml);
            var data = result.value;
            if (data.Any())
            {
                foreach (var item in data)
                {
                    item.Type = CalendarType.Contract;
                    item.StartDate = item.StartDate.ToLocalTime();
                    item.EndDate = item.StartDate.ToLocalTime();
                    item.Color = Color.FromHex("#6897BB");
                    item.BackgroundColor = Color.FromHex("#6897BB");
                    lstEvents.Add(item);
                }
            }
        }

        public async Task LoadContracts()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='salesorder'>
                                    <attribute name='name' alias='Title'/>
                                    <attribute name='statuscode' alias='State'/>
                                    <attribute name='createdon' alias='StartDate'/>
                                    <attribute name='salesorderid' alias='Id'/>
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='bsd_contractprinteddate' operator='not-null' />
                                      <condition attribute='bsd_signedcontractdate' operator='null' />
                                    </filter>
                                    <link-entity name='contactorders' from='salesorderid' to='salesorderid' visible='false' intersect='true'>
                                      <link-entity name='contact' from='contactid' to='contactid' alias='aa'>
                                        <filter type='and'>
                                          <condition attribute='contactid' operator='eq' value='{UserLogged.Id}' />
                                        </filter>
                                      </link-entity>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<CalendarModel>>("salesorders", fetchXml);
            var data = result.value;
            if (data.Any())
            {
                foreach (var item in data)
                {
                    item.Type = CalendarType.Contract;
                    item.StartDate = item.StartDate.ToLocalTime();
                    item.EndDate = item.StartDate.ToLocalTime();
                    item.Color = Color.FromHex("#A0DB8E");
                    item.BackgroundColor = Color.FromHex("#A0DB8E");
                    lstEvents.Add(item);
                }
            }
        }

        public void UpdateSelectedEvents(DateTime value)
        {
            this.selectedDateEvents.Clear();
            foreach (CalendarModel item in this.lstEvents)
            {
                try
                {
                    var date = value;
                    var recurrenceRule = item.RecurrenceRule;
                    if ((recurrenceRule == null && item.StartDate.CompareTo(date) >= 0 && item.StartDate.CompareTo(date.AddDays(1)) < 0)
                        || (recurrenceRule == null && item.EndDate.CompareTo(date) >= 0 && item.EndDate.CompareTo(date.AddDays(1)) < 0)
                        || (recurrenceRule == null && item.StartDate.CompareTo(date) < 0 && item.EndDate.CompareTo(date.AddDays(1)) >= 0)
                        || (recurrenceRule != null && recurrenceRule.Pattern.GetOccurrences(date, date, date).Any()))
                    {
                        this.selectedDateEvents.Add(item);
                    }
                }
                catch (Exception ex)
                {

                }

            }
        }

        public void UpdateSelectedEventsForWeekView(DateTime value)
        {
            this.selectedDateEvents.Clear();
            this.selectedDateEventsGrouped.Clear();

            var dayOfWeek = (int)value.DayOfWeek;
            for (int i = 1; i < 8; i++)
            {
                var currentDayOfWeek = i;
                var balance = currentDayOfWeek - dayOfWeek;
                var currentDate = value.AddDays(balance).Date;
                var checkHasValue = false;

                foreach (CalendarModel item in this.lstEvents)
                {
                    var date = currentDate;
                    var recurrenceRule = item.RecurrenceRule;
                    if ((recurrenceRule == null && item.StartDate.CompareTo(date) >= 0 && item.StartDate.CompareTo(date.AddDays(1)) < 0)
                        || (recurrenceRule == null && item.EndDate.CompareTo(date) >= 0 && item.EndDate.CompareTo(date.AddDays(1)) < 0)
                        || (recurrenceRule == null && item.StartDate.CompareTo(date) < 0 && item.EndDate.CompareTo(date.AddDays(1)) >= 0)
                        || (recurrenceRule != null && recurrenceRule.Pattern.GetOccurrences(date, date, date).Any()))
                    {
                        item.dateForGroupingWeek = date;
                        this.selectedDateEvents.Add(item);
                        checkHasValue = true;
                    }
                }
            }

            if (selectedDateEvents.Count > 0)
            {
                var sorted = from eventCalendar in selectedDateEvents
                             group eventCalendar by eventCalendar.dateForGroupingWeek.Value into dateGrouped
                             select new Grouping<DateTime, CalendarModel>(dateGrouped.Key, dateGrouped);
                this.selectedDateEventsGrouped = new ObservableCollection<Grouping<DateTime, CalendarModel>>(sorted);
            }
        }

        public async Task loadMeet(Guid id)
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                            <entity name='appointment'>
                                <attribute name='subject' />
                                <attribute name='statecode' />
                                <attribute name='createdby' />
                                <attribute name='statuscode' />
                                <attribute name='requiredattendees' />
                                <attribute name='prioritycode' />
                                <attribute name='scheduledstart' />
                                <attribute name='scheduledend' />
                                <attribute name='scheduleddurationminutes' />
                                <attribute name='bsd_mmeetingformuploaded' />
                                <attribute name='optionalattendees' />
                                <attribute name='isalldayevent' />
                                <attribute name='location' />
                                <attribute name='activityid' />
                                <attribute name='description' />
                                <order attribute='createdon' descending='true' />
                                <filter type='and'>
                                    <condition attribute='activityid' operator='eq' uitype='appointment' value='" + id + @"' />
                                </filter>               
                                <link-entity name='contact' from='contactid' to='bsd_customer' link-type='inner' alias='aa'>
                                  <attribute name='contactid' alias='contact_id' />                  
                                  <attribute name='fullname' alias='contact_name'/>
                                </link-entity>
                            </entity>
                          </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<MeetingModel>>("appointments", fetch);
            if (result == null || result.value == null)
                return;
            var data = result.value.FirstOrDefault();
            Meet = data;

            if (data.scheduledend != null && data.scheduledstart != null)
            {
                Meet.scheduledend = data.scheduledend.Value.ToLocalTime();
                Meet.scheduledstart = data.scheduledstart.Value.ToLocalTime();
            }

            if (Meet.contact_id != Guid.Empty)
            {
                Meet.Customer = new CustomerLookUp
                {
                    Name = Meet.contact_name
                };
            }

            if (Meet.statecode == 0)
                ShowGridButton = true;
            else
                ShowGridButton = false;
        }

        public async Task loadFromToMeet(Guid id)
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>
                                <entity name='appointment'>
                                    <attribute name='subject' />
                                    <attribute name='createdon' />
                                    <attribute name='activityid' />
                                    <order attribute='createdon' descending='false' />
                                    <filter type='and'>
                                        <condition attribute='activityid' operator='eq' value='" + id + @"' />
                                    </filter>
                                    <link-entity name='activityparty' from='activityid' to='activityid' link-type='inner' alias='ab'>
                                        <attribute name='partyid' alias='partyID'/>
                                        <attribute name='participationtypemask' alias='typemask'/>
                                      <link-entity name='account' from='accountid' to='partyid' link-type='outer' alias='partyaccount'>
                                        <attribute name='bsd_name' alias='account_name'/>
                                      </link-entity>
                                      <link-entity name='contact' from='contactid' to='partyid' link-type='outer' alias='partycontact'>
                                        <attribute name='fullname' alias='contact_name'/>
                                      </link-entity>
                                      <link-entity name='lead' from='leadid' to='partyid' link-type='outer' alias='partylead'>
                                        <attribute name='fullname' alias='lead_name'/>
                                      </link-entity>
                                      <link-entity name='systemuser' from='systemuserid' to='partyid' link-type='outer' alias='partyuser'>
                                        <attribute name='fullname' alias='user_name'/>
                                      </link-entity>
                                    </link-entity>
                                </entity>
                              </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<PartyModel>>("appointments", fetch);
            if (result == null || result.value == null)
                return;
            var data = result.value;
            if (data.Any())
            {
                List<string> required = new List<string>();
                List<string> optional = new List<string>();
                foreach (var item in data)
                {
                    if (item.typemask == 5) // from call
                    {
                        if (item.contact_name != null && item.contact_name != string.Empty)
                        {
                            required.Add(item.contact_name);
                        }
                        else if (item.account_name != null && item.account_name != string.Empty)
                        {
                            required.Add(item.account_name);
                        }
                        else if (item.lead_name != null && item.lead_name != string.Empty)
                        {
                            required.Add(item.lead_name);
                        }
                    }
                    else if (item.typemask == 6) // to call
                    {
                        if (item.contact_name != null && item.contact_name != string.Empty)
                        {
                            optional.Add(item.contact_name);
                        }
                        else if (item.account_name != null && item.account_name != string.Empty)
                        {
                            optional.Add(item.account_name);
                        }
                        else if (item.lead_name != null && item.lead_name != string.Empty)
                        {
                            optional.Add(item.lead_name);
                        }
                    }
                }
                Meet.required = string.Join(", ", required);
                Meet.optional = string.Join(", ", optional);
            }
        }

        public async Task<bool> UpdateStatusMeet()
        {
            Meet.statecode = 2;

            IDictionary<string, object> data = new Dictionary<string, object>();
            data["statecode"] = Meet.statecode;

            string path = "/appointments(" + Meet.activityid + ")";
            CrmApiResponse result = await CrmHelper.PatchData(path, data);
            if (result.IsSuccess)
            {
                ShowGridButton = false;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
