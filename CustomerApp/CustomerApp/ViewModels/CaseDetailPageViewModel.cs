using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CustomerApp.Datas;
using CustomerApp.Helper;
using CustomerApp.Models;
using CustomerApp.Settings;

namespace CustomerApp.ViewModels
{
    public class CaseDetailPageViewModel : BaseViewModel
    {
        public ObservableCollection<FloatButtonItem> ButtonCommandList { get; set; } = new ObservableCollection<FloatButtonItem>();
        public ObservableCollection<CasesModel> _listCase;
        public ObservableCollection<CasesModel> ListCase { get => _listCase; set { _listCase = value; OnPropertyChanged(nameof(ListCase)); } }

        private CaseModel _case;
        public CaseModel Case { get => _case; set { _case = value; OnPropertyChanged(nameof(Case)); } }

        public OptionSet _caseType;
        public OptionSet CaseType { get => _caseType; set { _caseType = value; OnPropertyChanged(nameof(CaseType)); } }

        public OptionSet _origin;
        public OptionSet Origin { get => _origin; set { _origin = value; OnPropertyChanged(nameof(Origin)); } }

        public OptionSet _statusCode;
        public OptionSet StatusCode { get => _statusCode; set { _statusCode = value; OnPropertyChanged(nameof(StatusCode)); } }

        private bool _showMoreCase;
        public bool ShowMoreCase { get => _showMoreCase; set { _showMoreCase = value; OnPropertyChanged(nameof(ShowMoreCase)); } }

        private bool _showFloatingButtonGroup = false;
        public bool ShowFloatingButtonGroup { get => _showFloatingButtonGroup; set { _showFloatingButtonGroup = value; OnPropertyChanged(nameof(ShowFloatingButtonGroup)); } }

        public Guid CaseId { get; set; }
        public int PageCase { get; set; } = 1;

        public CaseDetailPageViewModel()
        {
            ListCase = new ObservableCollection<CasesModel>();
        }

        public async Task LoadCaseInfor()
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='incident'>
                                    <attribute name='title' />
                                    <attribute name='incidentid' />
                                    <attribute name='caseorigincode' />
                                    <attribute name='statuscode' />
                                    <attribute name='statecode' />
                                    <attribute name='casetypecode' />
                                    <attribute name='description' />
                                  <order attribute='title' descending='false' />
                                  <filter type='and'>
                                      <condition attribute='incidentid' operator='eq'  value='{" + this.CaseId + @"}' />
                                  </filter>
                                  <link-entity name='account' from='accountid' to='customerid' visible='false' link-type='outer' alias='accounts'>
                                    <attribute name='bsd_name' alias='accountName'/>
                                    <attribute name='accountid' alias='accountId'/>
                                </link-entity>
                                <link-entity name='contact' from='contactid' to='customerid' visible='false' link-type='outer' alias='contacts'>
                                    <attribute name='bsd_fullname' alias='contactName'/>
                                    <attribute name='contactid' alias='contactId'/>
                                </link-entity>
                                <link-entity name='product' from='productid' to='productid' visible='false' link-type='outer' alias='products'>
                                  <attribute name='name' alias='unitName'/>
                                </link-entity>
                                <link-entity name='subject' from='subjectid' to='subjectid' visible='false' link-type='outer' >
                                  <attribute name='title' alias='subjectTitle'/>
                                </link-entity>
                                <link-entity name='incident' from='incidentid' to='parentcaseid' link-type='outer' alias='aa'>    
                                    <attribute name='title' alias='parentCaseTitle' />
                                </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<CaseModel>>("incidents", fetch);
            if (result == null || result.value == null)
                return;
            Case = result.value.FirstOrDefault();

            CaseType = CaseTypeData.GetCaseById(Case.casetypecode);
            Origin = CaseOriginData.GetOriginById(Case.caseorigincode);
            StatusCode = CaseStatusCodeData.GetCaseStatusCodeById(Case.statuscode.ToString());
        }

        public async Task LoadListCase()
        {
            string fetchXml = $@"<fetch version='1.0' count='3' page='{PageCase}' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='incident'>
                                    <attribute name='title' />
                                    <attribute name='casetypecode' />
                                    <order attribute='title' descending='false' />                               
                                    <link-entity name='account' from='accountid' to='customerid' visible='false' link-type='outer' alias='accounts'>
                                      <attribute name='bsd_name' alias='case_nameaccount'/>
                                    </link-entity>
                                    <link-entity name='contact' from='contactid' to='customerid' visible='false' link-type='outer' alias='contacts'>
                                      <attribute name='bsd_fullname' alias='case_namecontact'/>
                                    </link-entity>                               
                                    <filter type='and'>
                                        <condition attribute='parentcaseid' operator='eq' uitype='incident' value='{this.CaseId}' />
                                        <condition attribute='customerid' operator='eq' uitype='contact' value='{UserLogged.Id}' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<CasesModel>>("incidents", fetchXml);
            if (result == null || result.value.Count == 0)
            {
                ShowMoreCase = false;
                return;
            }
            var data = result.value;
            if (data.Count < 3)
            {
                ShowMoreCase = false;
            }
            else
            {
                ShowMoreCase = true;
            }
            foreach (var item in data)
            {
                ListCase.Add(item);
            }
        }

        public async Task<bool> UpdateCase()
        {
            string path = $"/incidents({Case.incidentid})";
            var content = await GetContent();
            CrmApiResponse apiResponse = await CrmHelper.PatchData(path, content);
            if (apiResponse.IsSuccess)
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
            data["statecode"] = Case.statecode;
            data["statuscode"] = Case.statuscode;
            return data;
        }
    }
}
