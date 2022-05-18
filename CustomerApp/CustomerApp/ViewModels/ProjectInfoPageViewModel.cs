﻿using CustomerApp.Datas;
using CustomerApp.Helper;
using CustomerApp.Models;
using CustomerApp.Settings;
using Stormlion.PhotoBrowser;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Firebase.Database;
using Firebase.Database.Query;

namespace CustomerApp.ViewModels
{
    public class ProjectInfoPageViewModel : BaseViewModel
    {
        FirebaseClient firebaseClient = new FirebaseClient("https://smsappcrm-default-rtdb.asia-southeast1.firebasedatabase.app/",
            new FirebaseOptions { AuthTokenAsyncFactory = () => Task.FromResult("kLHIPuBhEIrL6s3J6NuHpQI13H7M0kHjBRLmGEPF") });

        public ObservableCollection<CollectionData> Collections { get; set; } = new ObservableCollection<CollectionData>();

        public List<Photo> Photos { get; set; }
        private bool _showCollections = false;
        public bool ShowCollections { get => _showCollections; set { _showCollections = value; OnPropertyChanged(nameof(ShowCollections)); } }

        private int _totalMedia;
        public int TotalMedia { get => _totalMedia; set { _totalMedia = value; OnPropertyChanged(nameof(TotalMedia)); } }

        private int _totalPhoto;
        public int TotalPhoto { get => _totalPhoto; set { _totalPhoto = value; OnPropertyChanged(nameof(TotalPhoto)); } }
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        public List<ChartModel> unitChartModels { get; set; }
        public ObservableCollection<ChartModel> UnitChart { get; set; } = new ObservableCollection<ChartModel>();

        private ObservableCollection<QueuesModel> _listGiuCho;
        public ObservableCollection<QueuesModel> ListGiuCho { get => _listGiuCho; set { _listGiuCho = value; OnPropertyChanged(nameof(ListGiuCho)); } }

        private ObservableCollection<PhasesLanchModel> _listPhasesLanch;
        public ObservableCollection<PhasesLanchModel> ListPhasesLanch { get => _listPhasesLanch; set { _listPhasesLanch = value; OnPropertyChanged(nameof(ListPhasesLanch)); } }

        private EventModel _event;
        public EventModel Event { get => _event; set { _event = value; OnPropertyChanged(nameof(Event)); } }

        //private List<PDFModel> _listPDF;
        //public List<PDFModel> ListPDF { get => _listPDF; set { _listPDF = value; OnPropertyChanged(nameof(ListPDF)); } }

        public ObservableCollection<PDFModel> ListPDF { get; set; } = new ObservableCollection<PDFModel>();
        public Stream Stream { get; set; }

        private ProjectModel _project;
        public ProjectModel Project
        {
            get => _project;
            set
            {
                if (_project != value)
                { _project = value; OnPropertyChanged(nameof(Project)); }
            }
        }

        private OptionSet _projectType;
        public OptionSet ProjectType { get => _projectType; set { _projectType = value; OnPropertyChanged(nameof(ProjectType)); } }

        private OptionSet _propertyUsageType;
        public OptionSet PropertyUsageType { get => _propertyUsageType; set { _propertyUsageType = value; OnPropertyChanged(nameof(PropertyUsageType)); } }

        private OptionSet _handoverCoditionMinimum;
        public OptionSet HandoverCoditionMinimum { get => _handoverCoditionMinimum; set { _handoverCoditionMinimum = value; OnPropertyChanged(nameof(HandoverCoditionMinimum)); } }

        private bool _isShowBtnGiuCho;
        public bool IsShowBtnGiuCho { get => _isShowBtnGiuCho; set { _isShowBtnGiuCho = value; OnPropertyChanged(nameof(IsShowBtnGiuCho)); } }

        private int _numUnit = 0;
        public int NumUnit { get => _numUnit; set { _numUnit = value; OnPropertyChanged(nameof(NumUnit)); } }

        private int _soGiuCho = 0;
        public int SoGiuCho { get => _soGiuCho; set { _soGiuCho = value; OnPropertyChanged(nameof(SoGiuCho)); } }

        private int _soDatCoc = 0;
        public int SoDatCoc { get => _soDatCoc; set { _soDatCoc = value; OnPropertyChanged(nameof(SoDatCoc)); } }

        private int _soHopDong = 0;
        public int SoHopDong { get => _soHopDong; set { _soHopDong = value; OnPropertyChanged(nameof(SoHopDong)); } }

        private int _soBangTinhGia = 0;
        public int SoBangTinhGia { get => _soBangTinhGia; set { _soBangTinhGia = value; OnPropertyChanged(nameof(SoBangTinhGia)); } }

        private bool _showMoreBtnGiuCho;
        public bool ShowMoreBtnGiuCho { get => _showMoreBtnGiuCho; set { _showMoreBtnGiuCho = value; OnPropertyChanged(nameof(ShowMoreBtnGiuCho)); } }

        private bool _isHasEvent;
        public bool IsHasEvent { get => _isHasEvent; set { _isHasEvent = value; OnPropertyChanged(nameof(IsHasEvent)); } }

        public int ChuanBi { get; set; } = 0;
        public int SanSang { get; set; } = 0;
        public int GiuCho { get; set; } = 0;
        public int DatCoc { get; set; } = 0;
        public int DongYChuyenCoc { get; set; } = 0;
        public int DaDuTienCoc { get; set; } = 0;
        public int ThanhToanDot1 { get; set; } = 0;
        public int DaBan { get; set; } = 0;
        public int Booking { get; set; } = 0;
        public int Option { get; set; } = 0;
        public int SignedDA { get; set; } = 0;
        public int Qualified { get; set; } = 0;

        public bool IsLoadedGiuCho { get; set; }

        public int PageListGiuCho = 1;

        private ImageSource _ImageSource;
        public ImageSource ImageSource { get => _ImageSource; set { _ImageSource = value; OnPropertyChanged(nameof(ImageSource)); } }

        private StatusCodeModel _statusCode;
        public StatusCodeModel StatusCode { get => _statusCode; set { _statusCode = value; OnPropertyChanged(nameof(StatusCode)); } }

        private List<CollectionData> _listCollection;
        public List<CollectionData> ListCollection { get => _listCollection; set { _listCollection = value; OnPropertyChanged(nameof(ListCollection)); } }

        public ProjectInfoPageViewModel()
        {
            ListGiuCho = new ObservableCollection<QueuesModel>();
            ListPhasesLanch = new ObservableCollection<PhasesLanchModel>();
        }

        public async Task LoadData()
        {
            string FetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='bsd_project'>
                                <attribute name='bsd_projectid' />
                                <attribute name='bsd_projectcode' />
                                <attribute name='bsd_name' />
                                <attribute name='createdon' />
                                <attribute name='bsd_address' />
                                <attribute name='bsd_projecttype' />
                                <attribute name='bsd_propertyusagetype' />
                                <attribute name='bsd_depositpercentda' />
                                <attribute name='bsd_estimatehandoverdate' />
                                <attribute name='bsd_landvalueofproject' />
                                <attribute name='bsd_maintenancefeespercent' />
                                <attribute name='bsd_numberofmonthspaidmf' />
                                <attribute name='bsd_managementamount' />
                                <attribute name='bsd_bookingfee' />
                                <attribute name='bsd_depositamount' />
                                <attribute name='statuscode' />
                                <attribute name='bsd_queuesperunit' />
                                <attribute name='bsd_unitspersalesman' />
                                <attribute name='bsd_queueunitdaysaleman' />
                                <attribute name='bsd_longqueuingtime' />
                                <attribute name='bsd_shortqueingtime' />
                                <attribute name='bsd_projectslogo'/>
                                <attribute name='bsd_queueproject'/>
                                <attribute name='bsd_printagreement'/>
                                <order attribute='bsd_name' descending='false' />
                                <filter type='and'>
                                  <condition attribute='bsd_projectid' operator='eq' uitype='bsd_project' value='" + ProjectId.ToString() + @"' />
                                </filter>
                                <link-entity name='account' from='accountid' to='bsd_investor' visible='false' link-type='outer' alias='a_8924f6d5b214e911a97f000d3aa04914'>
                                  <attribute name='bsd_name' alias='bsd_investor_name' />
                                  <attribute name='accountid' alias='bsd_investor_id' />
                                </link-entity>
                              </entity>
                            </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ProjectModel>>("bsd_projects", FetchXml);
            if (result == null || result.value.Any() == false) return;
            Project = result.value.FirstOrDefault();
            this.StatusCode = Data.GetProjectStatusCodeById(Project.statuscode);
            //await LoadAllCollection();
        }

        public async Task CheckEvent()
        {
            // ham check su kien hide/show cua du an (show khi du an dang trong thoi gian dien ra su kien, va trang thai la "Approved")
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='bsd_event'>
                                <attribute name='createdon' />
                                <attribute name='statuscode' />
                                <attribute name='bsd_startdate' />
                                <attribute name='bsd_enddate' />
                                <attribute name='bsd_eventid' />
                                <order attribute='createdon' descending='true' />
                                <filter type='and'>
                                  <condition attribute='statuscode' operator='eq' value='100000000' />
                                  <condition attribute='bsd_project' operator='eq' uitype='bsd_project' value='{ProjectId}' />
                                </filter>
                              </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<EventModel>>("bsd_events", fetchXml);
            if (result == null || result.value.Any() == false) return;

            var data = result.value;
            foreach (var item in data)
            {
                if (item.bsd_startdate < DateTime.Now && item.bsd_enddate > DateTime.Now)
                {
                    IsHasEvent = true;
                    return;
                }
            }
        }

        public async Task LoadThongKe()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='product'>
                                <attribute name='name' />
                                <attribute name='productnumber' />
                                <attribute name='statecode' />
                                <attribute name='productstructure' />
                                <attribute name='statuscode' />
                                <attribute name='bsd_projectcode' />
                                <attribute name='createdon' />
                                <attribute name='bsd_unitscodesams' />
                                <attribute name='productid' />
                                <order attribute='createdon' descending='true' />
                                <filter type='and'>
                                    <condition attribute='statuscode' operator='not-in'>
                                        <value>0</value>
                                    </condition>
                                    <condition attribute='bsd_projectcode' operator='eq' uitype='bsd_project' value='" + this.ProjectId + @"'/>
                                  </filter>
                              </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<Unit>>("products", fetchXml);
            if (result == null || result.value.Any() == false)
            {
                ChuanBi++;
                SanSang++;
                GiuCho++;
                DatCoc++;
                DongYChuyenCoc++;
                DaDuTienCoc++;
                ThanhToanDot1++;
                DaBan++;
                Booking++;
                Option++;
                SignedDA++;
                Qualified++;
                IsShowBtnGiuCho = true;
            }
            else
            {
                IsShowBtnGiuCho = false;
                var data = result.value;
                NumUnit = data.Count;
                ChuanBi = data.Where(x => x.statuscode == 1).Count();
                SanSang = data.Where(x => x.statuscode == 100000000).Count();
                DaBan = data.Where(x => x.statuscode == 100000002).Count();
                GiuCho = data.Where(x => x.statuscode == 100000004).Count();
                //DatCoc = data.Where(x => x.statuscode == 100000006).Count();
                //DongYChuyenCoc = data.Where(x => x.statuscode == 100000005).Count();
                //DaDuTienCoc = data.Where(x => x.statuscode == 100000003).Count();
                //ThanhToanDot1 = data.Where(x => x.statuscode == 100000001).Count();
                //Booking = data.Where(x => x.statuscode == 100000007).Count();
                //Option = data.Where(x => x.statuscode == 100000010).Count();
                //SignedDA = data.Where(x => x.statuscode == 100000009).Count();
                //Qualified = data.Where(x => x.statuscode == 100000008).Count();
            }

            unitChartModels = new List<ChartModel>()
            {
                new ChartModel {Category ="Giữ chỗ",Value=GiuCho},
                new ChartModel { Category = "Đặt cọc", Value = DatCoc },
                new ChartModel {Category ="Đồng ý chuyển cọc",Value=DongYChuyenCoc },
                new ChartModel { Category = "Đã đủ tiền cọc", Value = DaDuTienCoc },
                new ChartModel { Category = "Option", Value = Option },
                new ChartModel {Category ="Thanh toán đợt 1",Value=ThanhToanDot1},
                new ChartModel { Category = "Signed D.A", Value = SignedDA },
                new ChartModel { Category = "Qualified", Value = Qualified },
                new ChartModel { Category = "Đã bán", Value =  DaBan},
                new ChartModel {Category ="Chuẩn bị", Value=ChuanBi},
                new ChartModel { Category = "Sẵn sàng", Value = SanSang },
                new ChartModel { Category = "Booking", Value = Booking },
            };
            foreach (var item in unitChartModels)
            {
                UnitChart.Add(item);
            }
        }

        public async Task LoadThongKeGiuCho()
        {
            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='opportunity'>
                                    <attribute name='name' />
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='bsd_project' operator='eq' uitype='bsd_project' value='{" + ProjectId + @"}' />
                                      <condition attribute='statuscode' operator='in'>
                                        <value>100000000</value>
                                        <value>100000002</value>
                                      </condition>
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<QueuesModel>>("opportunities", fetchXml);
            if (result == null || result.value.Any() == false) return;

            SoGiuCho = result.value.Count();
        }
        public async Task LoadThongKeHopDong()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='salesorder'>
                                <attribute name='name' />
                                <order attribute='createdon' descending='true' />
                                <filter type='and'>
                                    <condition attribute='statuscode' operator='ne' value='100000006' />
                                </filter>
                                <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' link-type='inner' alias='ad'>
                                  <filter type='and'>
                                    <condition attribute='bsd_projectid' operator='eq' value ='{ProjectId}'/>
                                  </filter>
                                </link-entity>
                              </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionEntry>>("salesorders", fetchXml);
            if (result == null || result.value.Any() == false) return;

            SoHopDong = result.value.Count();
        }
        public async Task LoadThongKeBangTinhGia()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='quote'>
                                <attribute name='name' />
                                <order attribute='createdon' descending='true' />
                                <filter type='and'>
                                  <condition attribute='statuscode' operator='eq' value='100000007' />
                                </filter>
                                <link-entity name='bsd_project' from='bsd_projectid' to='bsd_projectid' link-type='inner' alias='ae'>
                                  <filter type='and'>
                                    <condition attribute='bsd_projectid' operator='eq' value='{ProjectId}'/>
                                  </filter>
                                </link-entity>
                              </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<QuoteModel>>("quotes", fetchXml);
            if (result == null || result.value.Any() == false) return;
            SoBangTinhGia = result.value.Count();
        }
        public async Task LoadThongKeDatCoc()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='quote'>
                                <attribute name='name' />
                                <order attribute='createdon' descending='true' />
                                <filter type='and'>
                                  <condition attribute='statuscode' operator='in'>
                                    <value>100000000</value>
                                    <value>861450001</value>
                                    <value>861450002</value>
                                    <value>100000006</value>
                                    <value>3</value>
                                    <value>861450000</value>
                                  </condition>
                                </filter>
                                <link-entity name='bsd_project' from='bsd_projectid' to='bsd_projectid' link-type='inner' alias='ae'>
                                  <filter type='and'>
                                    <condition attribute='bsd_projectid' operator='eq' value='{ProjectId}'/>
                                  </filter>
                                </link-entity>
                              </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<QuoteModel>>("quotes", fetchXml);
            if (result == null || result.value.Any() == false) return;
            SoDatCoc = result.value.Count();
        }
        public async Task LoadGiuCho()
        {
            IsLoadedGiuCho = true;
            string fetchXml = $@"<fetch version='1.0' count='10' page='{PageListGiuCho}' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='opportunity'>
                                <attribute name='name' />
                                <attribute name='customerid' alias='customer_id'/>
                                <attribute name='bsd_bookingtime' />
                                <attribute name='statuscode' />
                                <attribute name='bsd_queuingexpired' />
                                <attribute name='opportunityid' />
                                <order attribute='bsd_bookingtime' descending='false' />
                                <filter type='and'>
                                  <condition attribute='statuscode' operator='in'>
                                    <value>100000002</value>
                                    <value>100000000</value>
                                  </condition>
                                </filter>
                                <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' link-type='inner' alias='ab'>
                                    <attribute name='bsd_name' alias='project_name'/>
                                  <filter type='and'>
                                    <condition attribute='bsd_projectid' operator='eq' value='{ProjectId}'/>
                                  </filter>
                                </link-entity>
                                <link-entity name='contact' from='contactid' to='customerid' visible='false' link-type='inner'>
                                   <attribute name='fullname'  alias='contact_name'/>
                                    <filter type='and'>
                                         <condition attribute='contactid' operator='eq' value='{UserLogged.Id}' />
                                    </filter>
                                </link-entity>
                              </entity>
                            </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<QueuesModel>>("opportunities", fetchXml);
            if (result == null || result.value.Any() == false) return;

            List<QueuesModel> data = result.value;
            ShowMoreBtnGiuCho = data.Count < 10 ? false : true;
            foreach (var item in data)
            {
                ListGiuCho.Add(item);
            }
        }
        public async Task LoadAllCollection()
        {
            var urlVideo234 = await CrmHelper.RetrieveImagesSharePoint<RetrieveMultipleApiResponse<GraphThumbnailsUrlModel>>($"{Config.OrgConfig.SharePointProjectId}/items/{8}/driveItem/thumbnails");
            if (urlVideo234 != null)
            {
                string url234 = urlVideo234.value.SingleOrDefault().large.url;// retri se lay duoc thumbnails gom 3 kich thuoc : large,medium,small
                this.Photos.Add(new Photo { URL = url234 });
            }

            if (ProjectId != null)
            {
                string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='sharepointdocument'>
                                    <attribute name='documentid' />
                                    <attribute name='fullname' />
                                    <attribute name='filetype' />
                                    <order attribute='relativelocation' descending='false' />
                                    <link-entity name='bsd_project' from='bsd_projectid' to='regardingobjectid' link-type='inner' alias='ad'>
                                      <filter type='and'>
                                        <condition attribute='bsd_projectid' operator='eq' value='{ProjectId}' />
                                      </filter>
                                    </link-entity>
                                  </entity>
                                </fetch>";
                var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<SharePonitModel>>("sharepointdocuments", fetchXml);

                if (result == null || result.value.Any() == false)
                {
                    ShowCollections = false;
                  //  return;
                }

                Photos = new List<Photo>();
                List<SharePonitModel> list = result.value;
                var videos = list.Where(x => x.filetype == "mp4" || x.filetype == "flv" || x.filetype == "m3u8" || x.filetype == "3gp" || x.filetype == "mov" || x.filetype == "avi" || x.filetype == "wmv").ToList();
                var images = list.Where(x => x.filetype == "jpg" || x.filetype == "jpeg" || x.filetype == "png").ToList();
                this.TotalMedia = videos.Count;
                this.TotalPhoto = images.Count;

                foreach (var item in videos)
                {
                    var urlVideo = await CrmHelper.RetrieveImagesSharePoint<RetrieveMultipleApiResponse<GraphThumbnailsUrlModel>>($"{Config.OrgConfig.SharePointProjectId}/items/{item.documentid}/driveItem/thumbnails");
                    string url = urlVideo.value.SingleOrDefault().large.url;// retri se lay duoc thumbnails gom 3 kich thuoc : large,medium,small
                    Collections.Add(new CollectionData { Id = item.documentid, MediaSourceId = item.documentid.ToString(), ImageSource = url, SharePointType = SharePointType.Video, Index = TotalMedia });
                }

                foreach (var item in images)
                {
                    var urlVideo = await CrmHelper.RetrieveImagesSharePoint<RetrieveMultipleApiResponse<GraphThumbnailsUrlModel>>($"{Config.OrgConfig.SharePointProjectId}/items/{item.documentid}/driveItem/thumbnails");
                    string url = urlVideo.value.SingleOrDefault().large.url;// retri se lay duoc thumbnails gom 3 kich thuoc : large,medium,small
                    this.Photos.Add(new Photo { URL = url });
                    Collections.Add(new CollectionData { Id = item.documentid, MediaSourceId = null, ImageSource = url, SharePointType = SharePointType.Image, Index = TotalMedia });
                }
            }
        }
        public async Task LoadDataEvent()
        {
            if (ProjectId == Guid.Empty) return;

            string FetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_event'>
                                    <attribute name='bsd_name' />
                                    <attribute name='bsd_startdate' />
                                    <attribute name='bsd_eventcode' />
                                    <attribute name='bsd_enddate' />
                                    <attribute name='bsd_eventid' />
                                    <order attribute='bsd_eventcode' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='statuscode' operator='eq' value='100000000' />
                                      <condition attribute='bsd_project' operator='eq' value='{ProjectId}' />
                                    </filter>
                                    <link-entity name='bsd_phaseslaunch' from='bsd_phaseslaunchid' to='bsd_phaselaunch' link-type='outer' alias='ab'>
                                      <attribute name='bsd_name' alias='bsd_phaselaunch_name'/>
                                    </link-entity>
                                  </entity>
                                </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<EventModel>>("bsd_events", FetchXml);
            if (result == null || result.value.Any() == false) return;
            Event = result.value.FirstOrDefault();
            if (Event.bsd_startdate.HasValue && Event.bsd_enddate.HasValue)
            {
                Event.bsd_startdate = Event.bsd_startdate.Value.ToLocalTime();
                Event.bsd_enddate = Event.bsd_enddate.Value.ToLocalTime();
            }
        }
        public async Task LoadPhasesLanch()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                    <entity name='bsd_phaseslaunch'>
                                        <attribute name='bsd_name' />
                                        <attribute name='createdon' />
                                        <attribute name='statuscode' />
                                        <attribute name='bsd_projectid' />
                                        <attribute name='bsd_startdate' />
                                        <attribute name='bsd_pricelistid' />
                                        <attribute name='bsd_enddate' />
                                        <attribute name='bsd_phaseslaunchid' />
                                        <order attribute='createdon' descending='true' />
                                        <filter type='and'>
                                            <condition attribute='bsd_projectid' operator='eq' value='{ProjectId}' />
                                        </filter>
                                    </entity>
                                </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<PhasesLanchModel>>("bsd_phaseslaunchs", fetchXml);
            if (result == null || result.value.Any() == false) return;

            List<PhasesLanchModel> data = result.value;
            foreach (var item in data)
            {
                ListPhasesLanch.Add(item);
            }
        }
        public async Task LoadPDF()
        {
            if (string.IsNullOrWhiteSpace(UserLogged.ListPdf))
            {
                ListPDF.Add(new PDFModel() { id = Guid.NewGuid(), name = "MultiPage PDF File.pdf" });
                ListPDF.Add(new PDFModel() { id = Guid.NewGuid(), name = "One Pearl Bank Community.pdf" });
                UserLogged.ListPdf = JsonConvert.SerializeObject(ListPDF);
            }
            else
            {
                var data = JsonConvert.DeserializeObject<List<PDFModel>>(UserLogged.ListPdf);
                foreach (var item in data)
                {
                    this.ListPDF.Add(item);
                }
            }
        }
        public async Task LoadCollection()
        {
            ShowCollections = true;
            ListCollection = new List<CollectionData>();
            Photos = new List<Photo>();
            List<CollectionData> list = new List<CollectionData>();

            try
            {
                var Items = (await firebaseClient
                                  .Child("FEV")
                                  .OnceAsync<CollectionData>()).Select(item => new CollectionData
                                  {
                                      Id = item.Object.Id,
                                      ImageSource = item.Object.ImageSource,
                                      MediaSourceId = item.Object.MediaSourceId,
                                      SharePointType = item.Object.SharePointType,
                                      Index = item.Object.Index,
                                  });
                foreach (var item in Items)
                {
                    string file = item.ImageSource;
                    string link = $"https://firebasestorage.googleapis.com/v0/b/smsappcrm.appspot.com/o/FEV%2Fimages%2F{file}?alt=media";
                    Photos.Add(new Photo { URL = link });
                    list.Add(new CollectionData { ImageSource = link, SharePointType = item.SharePointType });
                }
            }
            catch(Exception ex)
            {

            }

            list.Add(new CollectionData { ImageSource = "https://firebasestorage.googleapis.com/v0/b/customerapp-71c85.appspot.com/o/Screenshot%20(98).png?alt=media&token=b45c5601-fbe7-410a-ae1c-aa8592beb923", MediaSourceId = "https://firebasestorage.googleapis.com/v0/b/customerapp-71c85.appspot.com/o/Feliz%20en%20Vista%20-%20The%20Story%20Behind%20A%20Masterpiece.mp4?alt=media&token=01068c7e-bae9-40c8-b5f5-0ff34ee8e0fb", SharePointType = SharePointType.Video });
            list.Add(new CollectionData { ImageSource = "https://firebasestorage.googleapis.com/v0/b/customerapp-71c85.appspot.com/o/Screenshot%20(100).png?alt=media&token=4c64dbdc-2b2e-4ba0-8f3c-baf541a40f68", MediaSourceId = "https://firebasestorage.googleapis.com/v0/b/customerapp-71c85.appspot.com/o/Gi%E1%BB%9Bi%20thi%E1%BB%87u%20d%E1%BB%B1%20%C3%A1n%20%C4%91%E1%BB%89nh%20cao%20Feliz%20en%20Vista%20c%E1%BB%A7a%20CapitaLand%20-%20Crafting%20Tomorrow%E2%80%99s%20Beauty.mp4?alt=media&token=d9a4d8c1-f454-4661-9806-ecce3d26810c", SharePointType = SharePointType.Video });

            ListCollection = list;
            TotalMedia = list.Where(x=>x.SharePointType == SharePointType.Video).Count();
            TotalPhoto = list.Where(x => x.SharePointType == SharePointType.Image).Count();
        }
    }
}
