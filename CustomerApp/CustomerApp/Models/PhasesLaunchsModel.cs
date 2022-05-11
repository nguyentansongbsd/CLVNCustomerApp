using System;
using CustomerApp.Datas;

namespace CustomerApp.Models
{
    public class PhasesLaunchsModel
    {
        public Guid bsd_phaseslaunchid { get; set; }
        public string bsd_name { get; set; }
        public string statuscode { get; set; }
        public Guid bsd_projectid { get; set; }
        public DateTime bsd_startdate { get; set; }
        public DateTime bsd_enddate { get; set; }
        public string bsd_phaselaunchnumber { get; set; }
        public string project_name { get; set; }
        public string statuscode_format { get => Data.GetPhasesLanchStatusCodeById(statuscode)?.Name; }
        public string statuscode_color { get => Data.GetPhasesLanchStatusCodeById(statuscode)?.Background; }
    }
}
