using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CustomerApp.Datas;
using CustomerApp.Helper;
using CustomerApp.Models;
using CustomerApp.Settings;
using Xamarin.Forms;

namespace CustomerApp.ViewModels
{
    public class DashboardPageViewModel : BaseViewModel
    {
        public ObservableCollection<CalendarModel> DataEvents { get; set; } = new ObservableCollection<CalendarModel>();
        public ObservableCollection<ChartModel> DataMonthQueue { get; set; } = new ObservableCollection<ChartModel>();
        public ObservableCollection<ChartModel> DataMonthQuote { get; set; } = new ObservableCollection<ChartModel>();
        public ObservableCollection<ChartModel> DataMonthOptionEntry { get; set; } = new ObservableCollection<ChartModel>();
        public ObservableCollection<ChartModel> DataMonthUnit { get; set; } = new ObservableCollection<ChartModel>();

        private LoyaltyModel _loyalty;
        public LoyaltyModel Loyalty { get => _loyalty; set { _loyalty = value;OnPropertyChanged(nameof(Loyalty)); } }
        private string _loyaltyStatus;
        public string LoyaltyStatus { get => _loyaltyStatus; set { _loyaltyStatus = value; OnPropertyChanged(nameof(LoyaltyStatus)); } }

        private DateTime _dateBefor;
        public DateTime dateBefor { get => _dateBefor; set { _dateBefor = value; OnPropertyChanged(nameof(dateBefor)); } }
        public DateTime dateAfter { get; set; }

        private bool _isRefreshing;
        public bool IsRefreshing { get => _isRefreshing; set { _isRefreshing = value; OnPropertyChanged(nameof(IsRefreshing)); } }

        private int _numQueue;
        public int numQueue { get => _numQueue; set { _numQueue = value; OnPropertyChanged(nameof(numQueue)); } }
        private int _numQuote;
        public int numQuote { get => _numQuote; set { _numQuote = value; OnPropertyChanged(nameof(numQuote)); } }
        private int _numOptionEntry;
        public int numOptionEntry { get => _numOptionEntry; set { _numOptionEntry = value; OnPropertyChanged(nameof(numOptionEntry)); } }
        private int _numUnit;
        public int numUnit { get => _numUnit; set { _numUnit = value; OnPropertyChanged(nameof(numUnit)); } }

        public DateTime first_Month { get; set; }
        public DateTime second_Month { get; set; }
        public DateTime third_Month { get; set; }
        public DateTime fourth_Month { get; set; }

        public ICommand RefreshCommand => new Command(async () =>
        {
            IsRefreshing = true;
            await RefreshDashboard();
            IsRefreshing = false;
        });

        public DashboardPageViewModel()
        {
            dateBefor = DateTime.Now;
            DateTime threeMonthsAgo = DateTime.Now.AddMonths(-3);
            dateAfter = new DateTime(threeMonthsAgo.Year, threeMonthsAgo.Month, 1);
            first_Month = dateAfter;
            second_Month = dateAfter.AddMonths(1);
            third_Month = second_Month.AddMonths(1);
            fourth_Month = dateBefor;
        }

        public async Task LoadContactLoyalty()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='contact'>
                                    <attribute name='contactid' />
                                    <attribute name='bsd_totalamountofownership3years' />
                                    <attribute name='bsd_totalamountofownership' />
                                    <attribute name='bsd_loyaltystatus' />
                                    <attribute name='bsd_loyaltydate' />
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='contactid' operator='eq' value='{UserLogged.Id}'/>
                                    </filter>
                                    <link-entity name='bsd_membershiptier' from='bsd_membershiptierid' to='bsd_membershiptier' link-type='inner' alias='ad' >
                                        <attribute name='bsd_name' alias='membershiptier_name'/>
                                        <attribute name='bsd_membershiptierid' alias='membershiptier_id'/>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<LoyaltyModel>>("contacts", fetchXml);
            if (result == null && result.value == null) return;
            this.Loyalty = (result.value as List<LoyaltyModel>).SingleOrDefault();
            this.LoyaltyStatus = LoyaltyStatusData.GetLoyaltyById(this.Loyalty?.bsd_loyaltystatus)?.Value;
        }

        public async Task LoadQueueFourMonths()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                    <entity name='opportunity'>
                                        <attribute name='bsd_bookingtime' alias='Date'/>
                                        <attribute name='opportunityid' alias='Id' />
                                            <filter type='and'>
                                                <condition attribute='bsd_bookingtime' operator='on-or-after' value='{dateAfter.ToString("yyyy-MM-dd")}' />
                                                <condition attribute='statuscode' operator='in'>
                                                    <value>100000002</value>
                                                    <value>100000000</value>
                                                </condition>
                                                <condition attribute='parentcontactid' operator='eq' value='{UserLogged.Id}' />
                                            </filter>
                                    </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<DashboardChartModel>>("opportunities", fetchXml);
            if (result == null) return;

            var countQueueFr = result.value.Where(x => x.Date.Month == first_Month.Month).Count();
            ChartModel chartFirstMonth = new ChartModel() { Category = first_Month.ToString("MM/yyyy"), Value = countQueueFr };

            var countQueueSe = result.value.Where(x => x.Date.Month == second_Month.Month).Count();
            ChartModel chartSecondMonth = new ChartModel() { Category = second_Month.ToString("MM/yyyy"), Value = countQueueSe };

            var countQueueTh = result.value.Where(x => x.Date.Month == third_Month.Month).Count();
            ChartModel chartThirdMonth = new ChartModel() { Category = third_Month.ToString("MM/yyyy"), Value = countQueueTh };

            var countQueueFo = this.numQueue = result.value.Where(x => x.Date.Month == fourth_Month.Month).Count();
            ChartModel chartFourthMonth = new ChartModel() { Category = fourth_Month.ToString("MM/yyyy"), Value = countQueueFo };

            this.DataMonthQueue.Add(chartFirstMonth);
            this.DataMonthQueue.Add(chartSecondMonth);
            this.DataMonthQueue.Add(chartThirdMonth);
            this.DataMonthQueue.Add(chartFourthMonth);
        }

        public async Task LoadQuoteFourMonths()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='quote'>
                                    <attribute name='bsd_deposittime' alias='Date' />
                                    <attribute name='quoteid' alias='Id' />
                                    <filter type='and'>
                                      <condition attribute='statuscode' operator='in'>
                                        <value>3</value>
                                        <value>861450000</value>
                                        <value>4</value>
                                      </condition>
                                      <condition attribute='bsd_deposittime' operator='on-or-after' value='{dateAfter.ToString("yyyy-MM-dd")}' />
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
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<DashboardChartModel>>("quotes", fetchXml);
            if (result == null) return;

            var countQuoteFr = result.value.Where(x => x.Date.Month == first_Month.Month).Count();
            ChartModel chartFirstMonth = new ChartModel() { Category = first_Month.ToString("MM/yyyy"), Value = countQuoteFr };

            var countQuoteSe = result.value.Where(x => x.Date.Month == second_Month.Month).Count();
            ChartModel chartSecondMonth = new ChartModel() { Category = second_Month.ToString("MM/yyyy"), Value = countQuoteSe };

            var countQuoteTh = result.value.Where(x => x.Date.Month == third_Month.Month).Count();
            ChartModel chartThirdMonth = new ChartModel() { Category = third_Month.ToString("MM/yyyy"), Value = countQuoteTh };

            var countQuoteFo = this.numQuote = result.value.Where(x => x.Date.Month == fourth_Month.Month).Count();
            ChartModel chartFourthMonth = new ChartModel() { Category = fourth_Month.ToString("MM/yyyy"), Value = countQuoteFo };

            this.DataMonthQuote.Add(chartFirstMonth);
            this.DataMonthQuote.Add(chartSecondMonth);
            this.DataMonthQuote.Add(chartThirdMonth);
            this.DataMonthQuote.Add(chartFourthMonth);
        }

        public async Task LoadOptionEntryFourMonths()
        {
            // ngoại trừ các sts Terminated , 1st Installment, Option, Qualify, Signed D.A
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='salesorder'>
                                    <attribute name='createdon' alias='Date'/>
                                    <attribute name='quoteid' alias='Id'/>
                                    <filter type='and'>
                                      <condition attribute='statuscode' operator='not-in'>
                                        <value>100000006</value>
                                        <value>100000001</value>
                                        <value>100000000</value>
                                        <value>100000007</value>
                                        <value>100000008</value>
                                      </condition>
                                      <condition attribute='createdon' operator='on-or-after' value='{dateAfter.ToString("yyyy-MM-dd")}' />
                                      <condition attribute='bsd_signedcontractdate' operator='null' />
                                    </filter>
                                    <link-entity name='contactorders' from='salesorderid' to='salesorderid' visible='false' intersect='true'>
                                      <link-entity name='contact' from='contactid' to='contactid' alias='ab'>
                                        <filter type='and'>
                                          <condition attribute='contactid' operator='eq' value='{UserLogged.Id}'/>
                                        </filter>
                                      </link-entity>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<DashboardChartModel>>("salesorders", fetchXml);
            if (result == null) return;

            var countOptionEntryFr = result.value.Where(x => x.Date.Month == first_Month.Month).Count();
            ChartModel chartFirstMonth = new ChartModel() { Category = first_Month.ToString("MM/yyyy"), Value = countOptionEntryFr };

            var countOptionEntrySe = result.value.Where(x => x.Date.Month == second_Month.Month).Count();
            ChartModel chartSecondMonth = new ChartModel() { Category = second_Month.ToString("MM/yyyy"), Value = countOptionEntrySe };

            var countOptionEntryTh = result.value.Where(x => x.Date.Month == third_Month.Month).Count();
            ChartModel chartThirdMonth = new ChartModel() { Category = third_Month.ToString("MM/yyyy"), Value = countOptionEntryTh };

            var countOptionEntryFo = this.numOptionEntry = result.value.Where(x => x.Date.Month == fourth_Month.Month).Count();
            ChartModel chartFourthMonth = new ChartModel() { Category = fourth_Month.ToString("MM/yyyy"), Value = countOptionEntryFo };

            this.DataMonthOptionEntry.Add(chartFirstMonth);
            this.DataMonthOptionEntry.Add(chartSecondMonth);
            this.DataMonthOptionEntry.Add(chartThirdMonth);
            this.DataMonthOptionEntry.Add(chartFourthMonth);
        }

        public async Task LoadUnitFourMonths()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='product'>
                                    <attribute name='productid' alias='Id'/>
                                    <filter type='and'>
                                      <condition attribute='statuscode' operator='eq' value='100000002' />
                                    </filter>
                                    <link-entity name='salesorder' from='salesorderid' to='bsd_optionentry' link-type='inner'>
	                                    <attribute name='bsd_signedcontractdate' alias='Date'/>
                                          <filter type='and'>
                                            <condition attribute='bsd_signedcontractdate' operator='on-or-after' value='{dateAfter.ToString("yyyy-MM-dd")}' />
                                          </filter>
                                    </link-entity>
                                    <link-entity name='contact' from='contactid' to='bsd_customer' link-type='inner' alias='ac'>
                                      <filter type='and'>
                                        <condition attribute='contactid' operator='eq' value='{UserLogged.Id}'/>
                                      </filter>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<DashboardChartModel>>("products", fetchXml);
            if (result == null) return;

            var countUnitFr = result.value.Where(x => x.Date.Month == first_Month.Month).Count();
            ChartModel chartFirstMonth = new ChartModel() { Category = first_Month.ToString("MM/yyyy"), Value = countUnitFr };

            var countUnitSe = result.value.Where(x => x.Date.Month == second_Month.Month).Count();
            ChartModel chartSecondMonth = new ChartModel() { Category = second_Month.ToString("MM/yyyy"), Value = countUnitSe };

            var countUnitTh = result.value.Where(x => x.Date.Month == third_Month.Month).Count();
            ChartModel chartThirdMonth = new ChartModel() { Category = third_Month.ToString("MM/yyyy"), Value = countUnitTh };

            var countUnitFo = this.numUnit = result.value.Where(x => x.Date.Month == fourth_Month.Month).Count();
            ChartModel chartFourthMonth = new ChartModel() { Category = fourth_Month.ToString("MM/yyyy"), Value = countUnitFo };

            this.DataMonthUnit.Add(chartFirstMonth);
            this.DataMonthUnit.Add(chartSecondMonth);
            this.DataMonthUnit.Add(chartThirdMonth);
            this.DataMonthUnit.Add(chartFourthMonth);
        }

        private async Task RefreshDashboard()
        {
            this.Loyalty = null;
            this.DataMonthQueue.Clear();
            this.DataMonthQuote.Clear();
            this.DataMonthOptionEntry.Clear();
            this.DataMonthUnit.Clear();

            await Task.WhenAll(
                LoadContactLoyalty(),
                LoadQueueFourMonths(),
                LoadQuoteFourMonths(),
                LoadOptionEntryFourMonths(),
                LoadUnitFourMonths()
                );
        }

        public async Task LoadMeetings()
        {
            string fetch = $@"<fetch version='1.0' count='3' page='1' output-format='xml-platform' mapping='logical' distinct='false'>
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
                                      <condition attribute='scheduledstart' operator='today' />
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
                    item.Color = Color.FromHex("#d3ffce"); //Light Green
                    item.BackgroundColor = Color.White;
                    DataEvents.Add(item);
                }
            }
        }

        public async Task LoadQueues()
        {
            string fetchXml = $@"<fetch version='1.0' count='3' page='1' output-format='xml-platform' mapping='logical' distinct='false'>
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
                                      <condition attribute='bsd_bookingtime' operator='today' />
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
                    item.BackgroundColor = Color.White;
                    item.StartDate = item.StartDate.ToLocalTime();
                    item.EndDate = item.EndDate.ToLocalTime();
                    this.DataEvents.Add(item);
                }
            }
        }

        public async Task LoadDeposits()
        {
            string fetchXml = $@"<fetch version='1.0' count='3' page='1' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='quote'>
                                    <attribute name='name' alias='Title'/>
                                    <attribute name='statuscode' alias='State'/>
                                    <attribute name='quoteid' alias='ID'/>
                                    <attribute name='createdon' alias='StartDate'/>
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='statuscode' operator='eq' value='861450002' />
                                      <condition attribute='statuscode' operator='ne' value='6' />
                                      <condition attribute='createdon' operator='today' />
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
                    item.BackgroundColor = Color.White;
                    DataEvents.Add(item);
                }
            }
        }

        public async Task LoadInstallments()
        {
            string fetchXml = $@"<fetch version='1.0' count='3' page='1' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_paymentschemedetail'>
                                    <attribute name='bsd_paymentschemedetailid' alias='Id'/>
                                    <attribute name='bsd_name' alias='Title' />
                                    <attribute name='createdon' />
                                    <attribute name='bsd_duedate' alias='StartDate'/>
                                    <attribute name='statuscode' alias='State'/>
                                    <order attribute='bsd_name' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='statuscode' operator='eq' value='100000000' />
                                      <condition attribute='bsd_duedate' operator='today' />
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
                    item.BackgroundColor = Color.White;
                    DataEvents.Add(item);
                }
            }
        }

        public async Task LoadDepositsSigning()
        {
            string fetchXml = $@"<fetch version='1.0' count='3' page='1' output-format='xml-platform' mapping='logical' distinct='false'>
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
                                      <condition attribute='createdon' operator='today' />
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
                    item.BackgroundColor = Color.White;
                    DataEvents.Add(item);
                }
            }
        }

        public async Task LoadDepositAgreement()
        {
            string fetchXml = $@"<fetch version='1.0' count='3' page='1' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='salesorder'>
                                    <attribute name='name' alias='Title'/>
                                    <attribute name='statuscode' alias='State'/>
                                    <attribute name='createdon' alias='StartDate'/>
                                    <attribute name='salesorderid' alias='Id'/>
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='bsd_agreementdate' operator='not-null' />
                                      <condition attribute='bsd_signeddadate' operator='null' />
                                      <condition attribute='createdon' operator='today' />
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
                    item.BackgroundColor = Color.White;
                    DataEvents.Add(item);
                }
            }
        }

        public async Task LoadContracts()
        {
            string fetchXml = $@"<fetch version='1.0' count='3' page='1' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='salesorder'>
                                    <attribute name='name' alias='Title'/>
                                    <attribute name='statuscode' alias='State'/>
                                    <attribute name='createdon' alias='StartDate'/>
                                    <attribute name='salesorderid' alias='Id'/>
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='bsd_contractprinteddate' operator='not-null' />
                                      <condition attribute='bsd_signedcontractdate' operator='null' />
                                      <condition attribute='createdon' operator='today' />
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
                    item.BackgroundColor = Color.White;
                    DataEvents.Add(item);
                }
            }
        }
    }
}
