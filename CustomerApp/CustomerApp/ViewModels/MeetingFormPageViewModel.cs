using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerApp.Helper;
using CustomerApp.Models;
using CustomerApp.Settings;

namespace CustomerApp.ViewModels
{
    public class MeetingFormPageViewModel : BaseViewModel
    {
        public Guid MeetingId { get; set; }
        private MeetingModel _meetingModel;
        public MeetingModel MeetingModel { get => _meetingModel; set { if (_meetingModel != value) { _meetingModel = value; OnPropertyChanged(nameof(MeetingModel)); } } }

        private OptionSet _customer;
        public OptionSet Customer { get => _customer; set { _customer = value; OnPropertyChanged(nameof(Customer)); } }
        public bool _showButton;
        public bool ShowButton { get => _showButton; set { _showButton = value; OnPropertyChanged(nameof(ShowButton)); } }

        private string _required;
        public string Required { get => _required; set { _required = value; OnPropertyChanged(nameof(Required)); } }
        private string _optional;
        public string Optional { get => _optional; set { _optional = value; OnPropertyChanged(nameof(Optional)); } }

        private OptionSet _customerMapping;
        public OptionSet CustomerMapping { get => _customerMapping; set { _customerMapping = value; OnPropertyChanged(nameof(CustomerMapping)); } }

        public MeetingFormPageViewModel()
        {
            MeetingModel = new MeetingModel();
            CustomerMapping = new OptionSet() { Val = UserLogged.Id.ToString(), Label = UserLogged.User };
        }

        public async Task loadDataMeet()
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                  <entity name='appointment'>
                      <attribute name='subject' />
                      <attribute name='statecode' />
                      <attribute name='createdby' />
                      <attribute name='statuscode' />
                      <attribute name='scheduledstart' />
                      <attribute name='scheduledend' />
                      <attribute name='scheduleddurationminutes' />
                      <attribute name='isalldayevent' />
                      <attribute name='location' />
                      <attribute name='activityid' />
                      <attribute name='description' />
                      <order attribute='createdon' descending='true' />
                      <filter type='and'>
                          <condition attribute='activityid' operator='eq' uitype='appointment' value='" + this.MeetingId + @"' />
                      </filter>               
                      <link-entity name='contact' from='contactid' to='regardingobjectid' visible='false' link-type='outer' alias='contacts'>
                        <attribute name='contactid' alias='contact_id' />                  
                        <attribute name='fullname' alias='contact_name'/>
                      </link-entity>
                    <link-entity name='opportunity' from='opportunityid' to='regardingobjectid' link-type='outer' alias='ab'>
                        <attribute name='opportunityid' alias='queue_id'/>                  
                        <attribute name='name' alias='queue_name'/>
                    </link-entity>
                  </entity>
                </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<MeetingModel>>("appointments", fetch);
            if (result == null || result.value == null)
                return;
            this.MeetingModel = result.value.FirstOrDefault();

            if (MeetingModel.scheduledend != null && MeetingModel.scheduledstart != null)
            {
                MeetingModel.scheduledstart = MeetingModel.scheduledstart.Value.ToLocalTime();
                MeetingModel.scheduledend = MeetingModel.scheduledend.Value.ToLocalTime();
            }

            if (MeetingModel.contact_id != Guid.Empty)
            {
                Customer = new OptionSetFilter
                {
                    Val = MeetingModel.contact_id.ToString(),
                    Label = MeetingModel.contact_name
                };
            }
            
            if (MeetingModel.statecode == 0 || MeetingModel.statecode == 3)
                ShowButton = true;
            else
                ShowButton = false;
        }

        public async Task LoadRequiredAndOptional()
        {
            string xml_fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>
                  <entity name='appointment'>
                      <attribute name='activityid' />
                      <order attribute='createdon' descending='false' />
                      <filter type='and'>
                          <condition attribute='activityid' operator='eq' value='" + this.MeetingId + @"' />
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
            var _result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<PartyModel>>("appointments", xml_fetch);
            if (_result == null || _result.value == null)
                return;
            var _data = _result.value;
            if (_data.Any())
            {
                List<string> required = new List<string>();
                List<string> optional = new List<string>();
                foreach (var item in _data)
                {
                    if (item.typemask == 5)
                    {
                        required.Add(item.cutomer_name);
                    }
                    else if (item.typemask == 6)
                    {
                        optional.Add(item.cutomer_name);
                    }
                }
                Optional = string.Join(", ", optional);
                Required = string.Join(", ", required);
            }
        }

        public async Task<bool> UpdateMeeting()
        {
            var actualdurationminutes = Math.Round((MeetingModel.scheduledend.Value - MeetingModel.scheduledstart.Value).TotalMinutes);
            MeetingModel.scheduleddurationminutes = int.Parse(actualdurationminutes.ToString());
            string path = "/appointments(" + this.MeetingId + ")";
            var content = await this.getContent();
            CrmApiResponse result = await CrmHelper.PatchData(path, content);
            if (result.IsSuccess)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private async Task<object> getContent()
        {
            IDictionary<string, object> data = new Dictionary<string, object>();
            data["isalldayevent"] = MeetingModel.isalldayevent;
            data["scheduledstart"] = MeetingModel.scheduledstart.Value.ToUniversalTime();
            data["scheduledend"] = MeetingModel.scheduledend.Value.ToUniversalTime();
            data["actualdurationminutes"] = MeetingModel.scheduleddurationminutes;
            return data;
        }
    }
}
