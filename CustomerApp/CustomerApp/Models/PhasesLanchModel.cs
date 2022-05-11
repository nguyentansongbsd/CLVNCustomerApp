using CustomerApp.Datas;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerApp.Models
{
    public class PhasesLanchModel
    {
        public string bsd_name { get; set; }
        public DateTime createdon { get; set; }
        public Guid bsd_phaseslaunchid { get; set; }
        public DateTime startdate_event { get; set; }
        public DateTime enddate_event { get; set; }
        public string statuscode_event { get; set; }

        public Guid discount_id { get; set; }
        public string discount_name { get; set; }
        public Guid internel_id { get; set; }
        public string internel_name { get; set; }
        public Guid paymentscheme_id { get; set; }
        public string paymentscheme_name { get; set; }
        public Guid promotion_id { get; set; }
        public string promotion_name { get; set; }
        public DateTime bsd_enddate { get; set; }
        public string statuscode { get; set; }
        public string statuscode_format { get => Data.GetPhasesLanchStatusCodeById(statuscode)?.Name; }
        public string statuscode_color { get => Data.GetPhasesLanchStatusCodeById(statuscode)?.Background; }
        public Guid bsd_projectid { get; set; }
        public DateTime bsd_startdate { get; set; }
        public string bsd_times { get; set; }
        public string bsd_shorttimeminute { get; set; }
        public string bsd_releasetype { get; set; }
        public string bsd_releasetype_format { get => Data.GetPhasesLanchReleaseById(bsd_releasetype)?.Name; }
        public DateTime bsd_recoveryon { get; set; }
        public string bsd_recoveryby { get; set; }
        public string bsd_phaselaunchnumber { get; set; }
        public DateTime bsd_launchedon { get; set; }
        public string bsd_formofdistribution { get; set; }
        public string bsd_formofdistribution_format { get => Data.GetPhasesLanchFODById(bsd_formofdistribution)?.Name; }
        public decimal bsd_depositamount { get; set; }
        public string bsd_launchedfrom { get; set; }
        public string pricelist_name { get; set; }
        public string recoveryby_name { get; set; }
        public string launche_name { get; set; }
    }
}
