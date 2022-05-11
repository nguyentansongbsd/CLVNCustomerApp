using System;
using CustomerApp.Models;
using CustomerApp.Settings;
using Xamarin.Forms;

namespace CustomerApp.ViewModels
{
    public class CasesPageViewModel : ListViewBaseViewModel2<CasesModel>
    {
        public string Keyword { get; set; }
        public CasesPageViewModel()
        {
            PreLoadData = new Command(() =>
            {
                EntityName = "incidents";
                FetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' count='15' page='{Page}'>
                                <entity name='incident'>
                                    <attribute name='title' />
                                    <attribute name='statuscode' />
                                    <attribute name='casetypecode' />
                                    <attribute name='caseorigincode' />
                                    <attribute name='incidentid' />
                                    <order attribute='createdon' descending='true' />
                                    <link-entity name='account' from='accountid' to='customerid' visible='false' link-type='outer'>
                                        <attribute name='bsd_name' alias='case_nameaccount'/>
                                    </link-entity>
                                    <link-entity name='contact' from='contactid' to='customerid' visible='false' link-type='outer' >
                                      <attribute name='bsd_fullname' alias='case_namecontact'/>
                                    </link-entity>
                                     <filter type='and'>
                                          <filter type='or'>
                                              <condition attribute='title' operator='like' value='%25{Keyword}%25' />
                                              <condition attribute='customeridname' operator='like' value='%25{Keyword}%25' />
                                              <condition attribute='productidname' operator='like' value='%25{Keyword}%25' />
                                          </filter>
                                          <condition attribute='customerid' operator='eq' uitype='contact' value='{UserLogged.Id}' />
                                    </filter>         
                                </entity>
                            </fetch>";
            });
        }
    }
}
