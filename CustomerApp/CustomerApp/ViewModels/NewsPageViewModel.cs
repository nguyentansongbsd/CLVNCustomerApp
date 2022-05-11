using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerApp.Helper;
using CustomerApp.Models;

namespace CustomerApp.ViewModels
{
    public class NewsPageViewModel : BaseViewModel
    {
        private List<PhasesLaunchsModel> _phasesLaunchs;
        public List<PhasesLaunchsModel> PhasesLaunchs { get => _phasesLaunchs; set { _phasesLaunchs = value;OnPropertyChanged(nameof(PhasesLaunchs)); } }
        public NewsPageViewModel()
        {
        }

        public async Task LoadPhasesLaunchs()
        {
            string ferchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_phaseslaunch'>
                                    <attribute name='bsd_name' />
                                    <attribute name='createdon' />
                                    <attribute name='statuscode' />
                                    <attribute name='bsd_projectid' />
                                    <attribute name='bsd_startdate' />
                                    <attribute name='bsd_enddate' />
                                    <attribute name='bsd_phaseslaunchid' />
                                    <attribute name='bsd_phaselaunchnumber' />
                                    <order attribute='createdon' descending='true' />
                                    <link-entity name='bsd_project' from='bsd_projectid' to='bsd_projectid' link-type='inner' alias='aa'>
                                      <attribute name='bsd_name' alias='project_name'/>
                                      <attribute name='bsd_projectid' />
                                    </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<PhasesLaunchsModel>>("bsd_phaseslaunchs", ferchXml);
            if (result == null || result.value.Count == 0) return;
            this.PhasesLaunchs = result.value;
        }
    }
}
