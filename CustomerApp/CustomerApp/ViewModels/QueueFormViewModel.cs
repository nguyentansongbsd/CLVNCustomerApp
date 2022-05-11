using CustomerApp.Helper;
using CustomerApp.Helpers;
using CustomerApp.Models;
using CustomerApp.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerApp.ViewModels
{
    public class QueueFormViewModel : BaseViewModel
    {
        private QueuesModel _queue;
        public QueuesModel Queue { get => _queue; set { _queue = value; OnPropertyChanged(nameof(Queue)); } }
        private OptionSetFilter _customer;
        public OptionSetFilter Customer
        {
            get => _customer;
            set
            {
                if (_customer != value)
                {
                    _customer = value;
                    OnPropertyChanged(nameof(Customer));
                }
            }
        }

        private List<LookUpModel> _daiLyOptions;
        public List<LookUpModel> DaiLyOptions { get => _daiLyOptions; set { _daiLyOptions = value; OnPropertyChanged(nameof(DaiLyOptions)); } }

        private LookUpModel _daiLyOption;
        public LookUpModel DailyOption { get => _daiLyOption; set { _daiLyOption = value; OnPropertyChanged(nameof(DailyOption)); } }

        private List<LookUpModel> _listCollaborator;
        public List<LookUpModel> ListCollaborator { get => _listCollaborator; set { _listCollaborator = value; OnPropertyChanged(nameof(ListCollaborator)); } }

        private LookUpModel _collaborator;
        public LookUpModel Collaborator { get => _collaborator; set { _collaborator = value; OnPropertyChanged(nameof(Collaborator)); } }

        private List<LookUpModel> _listCustomerReferral;
        public List<LookUpModel> ListCustomerReferral { get => _listCustomerReferral; set { _listCustomerReferral = value; OnPropertyChanged(nameof(ListCustomerReferral)); } }

        private LookUpModel _customerReferral;
        public LookUpModel CustomerReferral { get => _customerReferral; set { _customerReferral = value; OnPropertyChanged(nameof(CustomerReferral)); } }
        public Guid idQueueDraft { get; set; } //StatusReason
        public string Error_update_queue { get; set; }
        public Guid UnitId { get; set; }

        private bool _isBusy;
        public bool isBusy { get => _isBusy; set { _isBusy = value; OnPropertyChanged(nameof(isBusy)); } }
        public QueueFormViewModel()
        {
            Queue = new QueuesModel();
            DaiLyOptions = new List<LookUpModel>();
            ListCollaborator = new List<LookUpModel>();
            ListCustomerReferral = new List<LookUpModel>();
        }

        public async Task LoadFromProject(Guid ProjectId)
        {
            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_project'>
                                    <attribute name='bsd_projectid' alias='bsd_project_id' />
                                    <attribute name='bsd_name' alias='bsd_project_name' />
                                    <attribute name='createdon' />
                                    <attribute name='bsd_bookingfee' alias='bsd_bookingf'/>
                                    <attribute name='bsd_shortqueingtime' alias='bsd_shorttime' />
                                    <attribute name='bsd_longqueuingtime' alias='bsd_longtime' />
                                    <order attribute='bsd_name' descending='false' />
                                    <filter type='and'>
                                        <condition attribute='bsd_projectid' operator='eq' value='{" + ProjectId.ToString() + @"}' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<QueuesModel>>("bsd_projects", fetchXml);
            if (result == null)
                return;
            var tmp = result.value.FirstOrDefault();
            if (tmp == null)
            {
                return;
            }
            this.Queue = tmp;
            Queue.bsd_queuingfee = Queue.bsd_bookingf;
            Queue._queue_createdon = DateTime.Now;
            Queue.bsd_queuingfee_format = StringFormatHelper.FormatCurrency(Queue.bsd_queuingfee);
        }

        public async Task LoadFromUnit(Guid UnitId)
        {
            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='product'>
                                    <attribute name='name' alias='bsd_units_name' />                                
                                    <attribute name='statuscode' alias='UnitStatusCode'/>
                                    <attribute name='bsd_projectcode' />                                 
                                    <attribute name='productid' alias='bsd_units_id' />
                                    <attribute name='bsd_queuingfee' alias='bsd_units_queuingfee' />
                                    <attribute name='bsd_phaseslaunchid' />
                                    <attribute name='pricelevelid' alias='pricelist_id'/>
                                    <attribute name='bsd_blocknumber' alias='bsd_block_id'/>
                                    <attribute name='bsd_floor' alias='bsd_floor_id'/>
                                    <attribute name='price' alias='unit_price'/>
                                    <attribute name='defaultuomid' alias='_defaultuomid_value' />
                                    <attribute name='transactioncurrencyid' alias='_transactioncurrencyid_value'/>
                                    <attribute name='bsd_taxpercent'/>
                                    <order attribute='createdon' descending='true' />
                                    <link-entity name='bsd_project' from='bsd_projectid' to='bsd_projectcode' link-type='outer' alias='aa'>
 	                                    <attribute name='bsd_projectid' alias='bsd_project_id' />
    	                                <attribute name='bsd_name' alias='bsd_project_name' />
	                                    <attribute name='bsd_bookingfee' alias='bsd_bookingf' />
                                        <attribute name='bsd_longqueuingtime' alias='bsd_longtime' />
                                        <attribute name='bsd_shortqueingtime' alias='bsd_shorttime'/>
                                    </link-entity>
                                    <link-entity name='bsd_phaseslaunch' from='bsd_phaseslaunchid' to='bsd_phaseslaunchid' visible='false' link-type='outer' alias='a_8d7b98e66ce2e811a94e000d3a1bc2d1'>
    	                                <attribute name='bsd_name' alias='bsd_phaseslaunch_name' />
    	                                <attribute name='bsd_phaseslaunchid' alias='bsd_phaseslaunch_id' />                   
                                    </link-entity>
                                    <filter type='and'>
                                        <condition attribute='productid' operator='eq' value='{" + UnitId.ToString() + @"}' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<QueuesModel>>("products", fetchXml);
            if (result == null)
                return;
            var tmp = result.value.FirstOrDefault();
            if (tmp == null)
            {
                return;
            }
            this.Queue = tmp;
            if (Queue.bsd_units_queuingfee > 0)
                Queue.bsd_queuingfee = Queue.bsd_units_queuingfee;
            else if (Queue.bsd_bookingf > 0)
                Queue.bsd_queuingfee = Queue.bsd_bookingf;
            Queue.bsd_queuingfee_format = StringFormatHelper.FormatCurrency(Queue.bsd_queuingfee);
        }

        public async Task<bool> SetQueueTime()
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='opportunity'>
                                <attribute name='name' />
                                <attribute name='customerid' />
                                <attribute name='estimatedvalue' />
                                <attribute name='statuscode' />
                                <attribute name='createdon' />
                                <attribute name='bsd_queuenumber' />
                                <attribute name='bsd_project' />
                                <attribute name='opportunityid' />
                                <attribute name='bsd_queuingexpired' alias='_queue_bsd_queuingexpired' />
                                <attribute name='bsd_bookingtime' alias='_queue_bsd_bookingtime' />
                                <order attribute='createdon' descending='true' />                               
                                <link-entity name='contact' from='contactid' to='parentcontactid' visible='false' link-type='outer' alias='a_7eff24578704e911a98b000d3aa2e890'>
                                      <attribute name='contactid' alias='contact_id' />
                                      <attribute name='bsd_fullname' alias='contact_name' />
                                </link-entity>
                                <link-entity name='account' from='accountid' to='parentaccountid' visible='false' link-type='outer' alias='a_77ff24578704e911a98b000d3aa2e890'>
                                      <attribute name='accountid' alias='account_id' />
                                      <attribute name='bsd_name' alias='account_name' />
                                </link-entity>
                                <filter type='and'>
                                    <condition attribute='bsd_queueforproject' operator='eq' value='0' />
                                    <condition attribute='bsd_units' operator='eq' uitype='product' value='{" + Queue.bsd_units_id + @"}' />
                                    <condition attribute='statuscode' operator='in'>
                                        <value>100000000</value>
                                        <value>100000002</value>
                                    </condition>
                                </filter>
                              </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<QueuesModel>>("opportunities", fetch);
            if (result == null || result.value == null)
                return false;
            var data = result.value;

            if (data.Where(x => x.account_id == Guid.Parse(Customer.Val)).ToList().Count > 0 || data.Where(x => x.contact_id == Guid.Parse(Customer.Val)).ToList().Count > 0)
            {
                return false;
            }
            return true;
        }
        public async Task LoadSalesAgent()
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='account'>
                                    <attribute name='name' alias='Name' />
                                    <attribute name='accountid' alias='Id' />
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                       <condition attribute='bsd_businesstypesys' operator='contain-values'>
                                         <value>100000002</value>
                                       </condition>                                
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<LookUpModel>>("accounts", fetch);
            if (result == null)
                return;
            var data = result.value;
            foreach (var item in data)
            {
                DaiLyOptions.Add(item);
            }
        }
        public async Task<Boolean> DeletLookup(string fieldName, Guid AccountId)
        {
            var result = await CrmHelper.SetNullLookupField("opportunities", AccountId, fieldName);
            return result.IsSuccess;
        }
        public async Task<string> createQueueDraft(bool isQueueProject, Guid id)
        {
            if (isQueueProject)
            {
                var data = new
                {
                    Command = "ProjectQue"
                };
                var res = await CrmHelper.PostData($"/bsd_projects({id})//Microsoft.Dynamics.CRM.bsd_Action_Project_QueuesForProject", data);

                if (res.IsSuccess)
                {
                    string str = res.Content.ToString();
                    string[] arrListStr = str.Split(',');
                    foreach (var item in arrListStr)
                    {
                        if (item.Contains("content") == true)
                        {
                            var itemformat = item.Replace("content", "").Replace(":", "").Replace("'", "").Replace("}", "").Replace('"', ' ').Trim();
                            if (Guid.Parse(itemformat) != Guid.Empty)
                            {
                                this.idQueueDraft = Guid.Parse(itemformat);
                                //await LoadQueueDraft(idQueueDraft);
                                return null;
                            }
                            else
                            {
                                this.idQueueDraft = Guid.Empty;
                                return res.ErrorResponse?.error.message;
                            }
                        }
                        //else
                        //    return res.ErrorResponse?.error.message;
                    }
                }
                else
                {
                    this.idQueueDraft = Guid.Empty;
                    return res.ErrorResponse?.error.message;
                }
                return res.ErrorResponse?.error.message;
            }
            else
            {
                var data = new
                {
                    Command = "Book"
                };

                var res = await CrmHelper.PostData($"/products({id})//Microsoft.Dynamics.CRM.bsd_Action_DirectSale", data);

                if (res.IsSuccess)
                {
                    string str = res.Content.ToString();
                    string[] arrListStr = str.Split(',');
                    foreach (var item in arrListStr)
                    {
                        if (item.Contains("content") == true)
                        {
                            var itemformat = item.Replace("content", "").Replace(":", "").Replace("'", "").Replace("}", "").Replace('"', ' ').Trim();
                            if (Guid.Parse(itemformat) != Guid.Empty)
                            {
                                this.idQueueDraft = Guid.Parse(itemformat);
                                //await LoadQueueDraft(idQueueDraft);
                                return null;
                            }
                            else
                            {
                                this.idQueueDraft = Guid.Empty;
                                return res.ErrorResponse?.error.message;
                            }
                        }
                        //else
                        //    return res.ErrorResponse?.error.message;
                    }
                }
                else
                {
                    this.idQueueDraft = Guid.Empty;
                    return res.ErrorResponse?.error.message;
                }
                return res.ErrorResponse?.error.message;
            }
        }

        public async Task<bool> UpdateQueue(Guid id)
        {
            if (id != Guid.Empty)
            {
                string path = "/opportunities(" + id + ")";
                var content = await this.getContent2();
                CrmApiResponse result = await CrmHelper.PatchData(path, content);
                if (result.IsSuccess)
                {
                    return true;
                }
                else
                {
                    Error_update_queue = result.ErrorResponse?.error.message;
                    return false;
                }
            }
            else
                return false;
        }

        private async Task<object> getContent2()
        {
            IDictionary<string, object> data = new Dictionary<string, object>();
            //  data["bsd_queuingfee"] = QueueFormModel.bsd_queuingfee;
            data["name"] = Queue.name;

            // customer là người login contact
            if (UserLogged.Id != null)
            {
                data["customerid_contact@odata.bind"] = $"/contacts({UserLogged.Id})";
            }

            data["budgetamount"] = Queue.budgetamount;
            if (Queue.bsd_units_id == Guid.Empty)
            {
                data["estimatedvalue"] = 0;
            }
            data["description"] = Queue.description;

            if (DailyOption == null || DailyOption.Id == Guid.Empty)
            {
                await DeletLookup("bsd_salesagentcompany", Queue.opportunityid);
            }
            else
            {
                data["bsd_salesagentcompany@odata.bind"] = $"/accounts({DailyOption.Id})";
            }

            if (Collaborator == null || Collaborator.Id == Guid.Empty)
            {
                await DeletLookup("bsd_collaborator", Queue.opportunityid);
            }
            else
            {
                data["bsd_collaborator@odata.bind"] = $"/contacts({Collaborator.Id})";
            }

            if (CustomerReferral == null || CustomerReferral.Id == Guid.Empty)
            {
                await DeletLookup("bsd_customerreferral_contact", Queue.opportunityid);
            }
            else
            {
                data["bsd_customerreferral_contact@odata.bind"] = $"/contacts({CustomerReferral.Id})";
            }

            data["bsd_nameofstaffagent"] = Queue.bsd_nameofstaffagent;

            //không có
            //if (UserLogged.Id != null)
            //{
            //    data["bsd_employee@odata.bind"] = "/bsd_employees(" + UserLogged.Id + ")";
            //}
            //if (UserLogged.ManagerId != Guid.Empty)
            //{
            //    data["ownerid@odata.bind"] = "/systemusers(" + UserLogged.ManagerId + ")";
            //}
            data["transactioncurrencyid@odata.bind"] = $"/transactioncurrencies(2366fb85-b881-e911-a83b-000d3a07be23)";
            return data;
        }

        public async Task LoadSalesAgentCompany()
        {
            string fetchphaseslaunch = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='bsd_phaseslaunch'>
                                <attribute name='bsd_name' />
                                <attribute name='bsd_locked' />
                                <attribute name='bsd_salesagentcompany' />
                                <attribute name='bsd_phaseslaunchid' />
                                <order attribute='bsd_name' descending='true' />
                                <filter type='and'>
                                    <condition attribute='bsd_phaseslaunchid' operator='eq' value='{Queue.bsd_phaseslaunch_id}' />
                                </filter>
                                <link-entity name='account' from='accountid' to='bsd_salesagentcompany' link-type='outer' alias='aw'>
                                    <attribute name='name' alias='salesagentcompany_name' />
                                </link-entity>
                              </entity>
                            </fetch>";
            var result_phasesLaunch = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<PhasesLaunch>>("bsd_phaseslaunchs", fetchphaseslaunch);

            string develop = $@"<link-entity name='bsd_project' from='bsd_investor' to='accountid' link-type='inner' alias='aj'>
                                                <filter type='and'>
                                                    <condition attribute='bsd_projectid' operator='eq' value='{Queue.bsd_project_id}' />
                                                </filter>
                                            </link-entity>";
            string all = $@"<link-entity name='bsd_projectshare' from='bsd_salesagent' to='accountid' link-type='inner' alias='az'>
                                                <filter type='and'>
                                                    <condition attribute='statuscode' operator='eq' value='1' />
                                                    <condition attribute='bsd_project' operator='eq' value='{Queue.bsd_project_id}' />
                                                </filter>
                                            </link-entity>";
            string sale_phasesLaunch = $@"<link-entity name='bsd_phaseslaunch' from='bsd_salesagentcompany' to='accountid' link-type='inner' alias='ak'>
                                                        <filter type='and'>
                                                            <condition attribute='bsd_phaseslaunchid' operator='eq' value='{Queue.bsd_phaseslaunch_id}' />
                                                         </filter>
                                                    </link-entity>";
            string isproject = $@"<filter type='and'>
                                       <condition attribute='bsd_businesstypesys' operator='contain-values'>
                                         <value>100000002</value>
                                       </condition>                                
                                    </filter>";

            if (result_phasesLaunch != null && result_phasesLaunch.value.Count > 0)
            {
                var phasesLaunch = result_phasesLaunch.value.FirstOrDefault();
                if (phasesLaunch.bsd_locked == false)
                {
                    if (string.IsNullOrWhiteSpace(phasesLaunch.salesagentcompany_name))
                    {
                        if (DaiLyOptions != null)
                        {
                            DaiLyOptions.AddRange(await LoadAccuntSales(all));
                            DaiLyOptions.AddRange(await LoadAccuntSales(develop));
                        }
                    }
                    else
                    {
                        if (DaiLyOptions != null)
                        {
                            DaiLyOptions.AddRange(await LoadAccuntSales(sale_phasesLaunch));
                            DaiLyOptions.AddRange(await LoadAccuntSales(develop));
                        }
                    }
                }
                else if (phasesLaunch.bsd_locked == true)
                {
                    if (string.IsNullOrWhiteSpace(phasesLaunch.salesagentcompany_name))
                    {
                        if (DaiLyOptions != null)
                        {
                            DaiLyOptions.AddRange(await LoadAccuntSales(develop));
                        }
                    }
                    else
                    {
                        if (DaiLyOptions != null)
                        {
                            DaiLyOptions.AddRange(await LoadAccuntSales(sale_phasesLaunch));
                        }
                    }
                }

            }
            else
            {
                if (DaiLyOptions != null)
                {
                    DaiLyOptions.AddRange(await LoadAccuntSales(all));
                    DaiLyOptions.AddRange(await LoadAccuntSales(develop));
                }
            }
        }

        public async Task<List<LookUpModel>> LoadAccuntSales(string filter)
        {
            List<LookUpModel> list = new List<LookUpModel>();
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='account'>
                                    <attribute name='name' alias='Name' />
                                    <attribute name='accountid' alias='Id' />
                                    <order attribute='createdon' descending='true' />
                                    " + filter + @"
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<LookUpModel>>("accounts", fetch);
            if (result != null && result.value.Count != 0)
            {
                var data = result.value;
                foreach (var item in data)
                {
                    list.Add(item);
                }
            }
            return list;
        }

        public async Task LoadCollaboratorLookUp()
        {
            //<condition attribute='bsd_employee' operator='eq' uitype='bsd_employee' value='" + UserLogged.Id + @"' />
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                  <entity name='contact'>
                    <attribute name='contactid' alias='Id' />
                    <attribute name='fullname' alias='Name' />
                    <order attribute='createdon' descending='true' />                   
                    <filter type='and'>
                        <condition attribute='bsd_type' operator='eq' value='100000001' />
                    </filter>
                  </entity>
                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<LookUpModel>>("contacts", fetch);
            if (result == null || result.value.Count == 0)
                return;
            var data = result.value;
            foreach (var item in data)
            {
                ListCollaborator.Add(item);
            }
        }

        public async Task LoadCustomerReferralLookUp()
        {
            //<condition attribute='bsd_employee' operator='eq' uitype='bsd_employee' value='" + UserLogged.Id + @"' />
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                  <entity name='contact'>
                    <attribute name='contactid' alias='Id' />
                    <attribute name='fullname' alias='Name' />
                    <order attribute='createdon' descending='true' />                   
                    <filter type='and'>
                        <condition attribute='bsd_type' operator='eq' value='100000000' />
                    </filter>
                  </entity>
                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<LookUpModel>>("contacts", fetch);
            if (result == null || result.value.Count == 0)
                return;
            var data = result.value;
            foreach (var item in data)
            {
                ListCustomerReferral.Add(item);
            }
        }

        public async Task updateStatusUnit()
        {
            IDictionary<string, object> data = new Dictionary<string, object>();

            string fetchSalesorder = @"<fetch>
                                  <entity name='salesorder'>
                                    <attribute name='statecode' />
                                    <attribute name='statuscode' />
                                    <link-entity name='product' from='productid' to='bsd_unitnumber'>
                                      <filter>
                                        <condition attribute='productid' operator='eq' value='{" + Queue.bsd_units_id + @"}'/>
                                      </filter>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            var resultSalesorder = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ContractModel>>("salesorders", fetchSalesorder);
            if (resultSalesorder != null || resultSalesorder.value.Count > 0)
                return;

            string fetchQuote = @"<fetch>
                                  <entity name='quote'>
                                    <attribute name='statecode' />
                                    <attribute name='statuscode' />
                                    <link-entity name='product' from='productid' to='bsd_unitno'>
                                      <filter>
                                        <condition attribute='productid' operator='eq' value='{" + Queue.bsd_units_id + @"}'/>
                                      </filter>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            var resultQuote = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ReservationModel>>("quotes", fetchQuote);
            if (resultQuote != null || resultQuote.value.Count > 0)
            {
                if (resultQuote.value.FirstOrDefault(x => x.statuscode == 3 || x.statuscode == 861450000) != null)
                {
                    data["statecode"] = 0;
                    data["statuscode"] = 100000003;
                }
                else if (resultQuote.value.FirstOrDefault(x => x.statuscode == 4) != null)
                {
                    data["statecode"] = 0;
                    data["statuscode"] = 100000010;
                }
            }
            else
            {
                string fetchQueue = @"<fetch>
                                          <entity name='opportunity'>
                                            <attribute name='name' />
                                            <attribute name='statuscode' />
                                            <attribute name='bsd_phaselaunch' alias='_bsd_phaselaunch_value' />
                                            <filter type='and'>
                                              <condition attribute='bsd_units' operator='eq' value='{" + Queue.bsd_units_id + @"}'/>
                                            </filter>
                                          </entity>
                                        </fetch>";
                var resultQueue = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<QueuesModel>>("opportunities", fetchQueue);
                if (resultQueue != null || resultQueue.value.Count > 0)
                {
                    if (resultQueue.value.FirstOrDefault(x => x.statuscode == 100000000 || x.statuscode == 100000008) != null)
                    {
                        data["statecode"] = 0;
                        data["statuscode"] = 100000004;
                    }
                    else if (resultQueue.value.FirstOrDefault(x => x.statuscode == 100000002) != null)
                    {
                        data["statecode"] = 0;
                        data["statuscode"] = 100000007;
                    }
                    else
                    {
                        var queue = resultQueue.value.FirstOrDefault(x => x.statuscode == 4 || x.statuscode == 100000007 || x.statuscode == 100000009);
                        if (queue != null && queue._bsd_phaselaunch_value != Guid.Empty)
                        {
                            data["statecode"] = 0;
                            data["statuscode"] = 100000000;
                        }
                        else if (resultQueue.value.FirstOrDefault(x => x.statuscode == 4 || x.statuscode == 100000007) != null)
                        {
                            data["statecode"] = 0;
                            data["statuscode"] = 1;
                        }
                    }
                }
            }

            string path = "/products(" + Queue.bsd_units_id + ")";
            CrmApiResponse result = await CrmHelper.PatchData(path, data);
        }

        public void test(string a)
        {
            if (a.Contains("_moblie"))
            {
                a.Replace("_moblie", null);
                // theem don vi tien te
            }
            // chay book nhu binh thuong

        }
    }
}
