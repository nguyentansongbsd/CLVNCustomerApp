using CustomerApp.Helper;
using CustomerApp.Models;
using CustomerApp.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerApp.ViewModels
{
    public class FeedbackPageViewModel : BaseViewModel
    {
        private ContactModel _contact;
        public ContactModel Contact { get => _contact; set { _contact = value; OnPropertyChanged(nameof(Contact)); } }

        private string _subject;
        public string Subject { get => _subject; set { _subject = value; OnPropertyChanged(nameof(Subject)); } }

        private string _content;
        public string Content { get => _content; set { _content = value; OnPropertyChanged(nameof(Content)); } }

        public FeedbackPageViewModel()
        {
            
        }
        //load thông tin contact
        public async Task LoadContact()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='contact'>
                                    <attribute name='bsd_fullname' />
                                    <attribute name='mobilephone' />
                                    <attribute name='bsd_identitycardnumber' />
                                    <attribute name='gendercode' />
                                    <attribute name='emailaddress1' />
                                    <attribute name='createdon' />
                                    <attribute name='birthdate' />
                                    <attribute name='contactid' />
                                    <attribute name='bsd_postalcode' />
                                    <attribute name='bsd_housenumberstreet' />
                                    <attribute name='bsd_contactaddress' />
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='contactid' operator='eq' value='{UserLogged.Id}'/>
                                    </filter>
                                    <link-entity name='bsd_country' from='bsd_countryid' to='bsd_country' visible='false' link-type='outer' alias='a_8b5241be19dbeb11bacb002248168cad'>
                                        <attribute name='bsd_countryid' alias='country_id'/>
                                        <attribute name='bsd_countryname' alias='country_name'/>
                                        <attribute name='bsd_nameen'  alias='country_name_en'/>
                                    </link-entity>
                                    <link-entity name='new_province' from='new_provinceid' to='bsd_province' visible='false' link-type='outer' alias='a_8fd440dc19dbeb11bacb002248168cad'>
                                        <attribute name='new_provinceid' alias='province_id'/>
                                        <attribute name='new_name' alias='province_name'/>
                                        <attribute name='bsd_nameen'  alias='province_name_en'/>
                                    </link-entity>
                                    <link-entity name='new_district' from='new_districtid' to='bsd_district' visible='false' link-type='outer' alias='a_50d440dc19dbeb11bacb002248168cad'>
                                        <attribute name='new_districtid' alias='district_id'/>
                                        <attribute name='new_name' alias='district_name'/>
                                        <attribute name='bsd_nameen'  alias='district_name_en'/>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ContactModel>>("contacts", fetchXml);
            if (result == null || result.value.Any() == false) return;

            var data = result.value.SingleOrDefault();
            if (data.mobilephone.StartsWith("+84"))
            {
                data.mobilephone = data.mobilephone.Replace("+84", "").Replace(" ", "");
            }
            else if(data.mobilephone.StartsWith("84"))
            {
                data.mobilephone = data.mobilephone.Replace("84", "").Replace(" ", "");
            }
            Contact = result.value.SingleOrDefault();
        }
    }
}
