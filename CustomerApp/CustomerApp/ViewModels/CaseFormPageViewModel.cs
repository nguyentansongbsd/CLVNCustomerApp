using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerApp.Helper;
using CustomerApp.Models;
using CustomerApp.Settings;

namespace CustomerApp.ViewModels
{
    public class CaseFormPageViewModel : BaseViewModel
    {
        private CaseFromModel _caseModel;
        public CaseFromModel CaseModel { get => _caseModel; set { _caseModel = value; OnPropertyChanged(nameof(CaseModel)); } }

        private List<OptionSet> _caseTypes;
        public List<OptionSet> CaseTypes { get => _caseTypes; set { _caseTypes = value; OnPropertyChanged(nameof(CaseTypes)); } }
        private OptionSet _caseType;
        public OptionSet CaseType { get => _caseType; set { _caseType = value; OnPropertyChanged(nameof(CaseType)); } }

        private List<OptionSet> _subjects;
        public List<OptionSet> Subjects { get => _subjects; set { _subjects = value; OnPropertyChanged(nameof(Subjects)); } }
        private OptionSet _subject;
        public OptionSet Subject { get => _subject; set { _subject = value; OnPropertyChanged(nameof(Subject)); } }

        private List<OptionSet> _caseOrigins;
        public List<OptionSet> CaseOrigins { get => _caseOrigins; set { _caseOrigins = value; OnPropertyChanged(nameof(CaseOrigins)); } }
        private OptionSet _caseOrigin;
        public OptionSet CaseOrigin { get => _caseOrigin; set { _caseOrigin = value; OnPropertyChanged(nameof(CaseOrigin)); } }

        private List<OptionSet> _caseLienQuans;
        public List<OptionSet> CaseLienQuans { get => _caseLienQuans; set { _caseLienQuans = value; OnPropertyChanged(nameof(CaseLienQuans)); } }
        private OptionSet _caseLienQuan;
        public OptionSet CaseLienQuan { get => _caseLienQuan; set { _caseLienQuan = value; OnPropertyChanged(nameof(CaseLienQuan)); } }

        private List<OptionSet> _projects;
        public List<OptionSet> Projects { get => _projects; set { _projects = value; OnPropertyChanged(nameof(Projects)); } }
        private OptionSet _project;
        public OptionSet Project { get => _project; set { _project = value; OnPropertyChanged(nameof(Project)); } }

        private List<OptionSet> _units;
        public List<OptionSet> Units { get => _units; set { _units = value; OnPropertyChanged(nameof(Units)); } }
        private OptionSet _unit;
        public OptionSet Unit { get => _unit; set { _unit = value; OnPropertyChanged(nameof(Unit)); } }

        private string _customer;
        public string Customer { get => _customer; set { _customer = value; OnPropertyChanged(nameof(Customer)); } }

        public CaseFormPageViewModel()
        {
            CaseModel = new CaseFromModel();
            Customer = UserLogged.User;
        }

        public async Task LoadSubjects()
        {
            string fetchXml = @"<fetch version='1.0'  output-format='xml-platform' mapping='logical' distinct='false'>    
                                <entity name='subject'>      
                                    <attribute name='title' alias ='Label'/>
                                    <attribute name='subjectid' alias ='Val'/>
                                    <order attribute='createdon' descending='true' />
                                </entity>  
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("subjects", fetchXml);
            if (result == null || result.value.Count == 0) return;
            this.Subjects = result.value;
        }

        public async Task LoadCaseLienQuan()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='incident'>
                                    <attribute name='title' alias ='Label'/>
                                    <attribute name='incidentid' alias= 'Val'/>
                                    <order attribute='createdon' descending='false' />
                                    <filter type='and'>
                                       <condition attribute='customerid' operator='eq' uitype='contact' value='{UserLogged.Id}' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("incidents", fetchXml);
            if (result == null || result.value.Count == 0) return;
            this.CaseLienQuans = result.value;
        }

        public async Task<List<OptionSet>> LoadProjects(string filterByUnitId = null)
        {
            string codition = string.Empty;
            codition = !string.IsNullOrWhiteSpace(filterByUnitId) ? $@"<link-entity name='product' from='bsd_projectcode' to='bsd_projectid' link-type='inner' alias='ab'>
                                                                          <filter type='and'>   
                                                                            <condition attribute='productid' operator='eq' uitype='product' value='{filterByUnitId}' />
                                                                          </filter>
                                                                        </link-entity>" : null;
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_project'>
                                    <attribute name='bsd_projectid' alias='Val' />
                                    <attribute name='bsd_name' alias='Label' />
                                    <order attribute='createdon' descending='false' />
                                    {codition}
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("bsd_projects", fetchXml);
            if (result == null || result.value.Count == 0) return null;
            return result.value;
        }

        public async Task LoadUnits()
        {
            if (Project == null) return;
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='product'>
                                <attribute name='name' alias='Label'/>
                                <attribute name='productid' alias='Val'/>
                                <order attribute='createdon' descending='true' />
                                <filter type='and'>
                                  <condition attribute='bsd_projectcode' operator='eq'  uitype='bsd_project' value='{Project.Val}' />
                                </filter>
                              </entity>
                            </fetch>";
            var resutl = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("products", fetchXml);
            if (resutl == null || resutl.value.Count == 0) return;
            this.Units = resutl.value;
        }

        public async Task<bool> CreateCase()
        {
            string path = "/incidents";
            CaseModel.incidentid = Guid.NewGuid();
            var content = await GetContent();
            CrmApiResponse result = await CrmHelper.PostData(path, content);
            if (result.IsSuccess)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private async Task<object> GetContent()
        {
            IDictionary<string, object> data = new Dictionary<string, object>();
            data["incidentid"] = CaseModel.incidentid.ToString();
            data["casetypecode"] = CaseType.Val;
            data["title"] = CaseModel.title;
            data["caseorigincode"] = CaseOrigin != null ? CaseOrigin.Val : null;
            data["description"] = CaseModel.description;

            data["customerid_contact@odata.bind"] = "/contacts(" + UserLogged.Id + ")";

            if (Subject == null)
            {
                await DeletLookup("subjectid", CaseModel.incidentid);
            }
            else
            {
                data["subjectid@odata.bind"] = "/subjects(" + Subject.Val + ")";
            }

            if (CaseLienQuan == null)
            {
                await DeletLookup("parentcaseid", CaseModel.incidentid);
            }
            else
            {
                data["parentcaseid@odata.bind"] = $"incidents({CaseLienQuan.Val})";
            }

            if (Unit == null)
            {
                await DeletLookup("productid", CaseModel.incidentid);
            }
            else
            {
                data["productid@odata.bind"] = "/products(" + Unit.Val + ")";
            }

            if (UserLogged.ManagerId != Guid.Empty)
            {
                data["ownerid@odata.bind"] = "/systemusers(" + UserLogged.ManagerId + ")";
            }

            return data;
        }

        private async Task<Boolean> DeletLookup(string fieldName, Guid IncidentId)
        {
            var result = await CrmHelper.SetNullLookupField("incidents", IncidentId, fieldName);
            return result.IsSuccess;
        }
    }
}
