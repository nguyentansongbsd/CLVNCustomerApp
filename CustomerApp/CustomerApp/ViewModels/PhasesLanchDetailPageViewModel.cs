using CustomerApp.Helper;
using CustomerApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerApp.ViewModels
{
    public class PhasesLanchDetailPageViewModel : BaseViewModel
    {
        private PhasesLanchModel _phasesLanch;
        public PhasesLanchModel PhasesLanch { get => _phasesLanch; set { _phasesLanch = value; OnPropertyChanged(nameof(PhasesLanch)); } }
        public PhasesLanchDetailPageViewModel()
        {

        }
        public async Task LoadPhasesLanch(Guid phaseslaunchid)
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
                                        <attribute name='bsd_times' />
                                        <attribute name='bsd_shorttimeminute' />
                                        <attribute name='bsd_releasetype' />
                                        <attribute name='bsd_recoveryon' />
                                        <attribute name='bsd_recoveryby' />
                                        <attribute name='bsd_phaselaunchnumber' />
                                        <attribute name='bsd_launchedon' />
                                        <attribute name='bsd_launchedfrom' />
                                        <attribute name='bsd_formofdistribution' />
                                        <attribute name='bsd_depositamount' />
                                        <order attribute='createdon' descending='true' />
                                        <filter type='and'>
                                            <condition attribute='bsd_phaseslaunchid' operator='eq' value='{phaseslaunchid}' />
                                        </filter>
                                        <link-entity name='pricelevel' from='pricelevelid' to='bsd_pricelistid' link-type='outer'>
                                            <attribute name='name' alias='pricelist_name'/>
                                        </link-entity>
                                        <link-entity name='systemuser' from='systemuserid' to='bsd_recoveryby' link-type='outer'>
                                            <attribute name='fullname' alias='recoveryby_name'/>
                                        </link-entity>
                                        <link-entity name='systemuser' from='systemuserid' to='bsd_launchedfrom' link-type='outer'>
                                            <attribute name='fullname' alias='launche_name'/>
                                        </link-entity>
                                    </entity>
                                </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<PhasesLanchModel>>("bsd_phaseslaunchs", fetchXml);
            if (result == null || result.value.Any() == false) return;

            PhasesLanch = result.value.FirstOrDefault();
        }
    }
}
