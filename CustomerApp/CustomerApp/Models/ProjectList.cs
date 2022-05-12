using System;
using System.Collections.Generic;
using System.Text;
using CustomerApp.Datas;

namespace CustomerApp.Models
{
    public class ProjectList
    {
        public string bsd_projectcode { get; set; }
        public string bsd_name { get; set; }
        public Decimal? bsd_landvalueofproject { get; set; }
        public DateTime? bsd_esttopdate { get; set; }
        public DateTime? bsd_acttopdate { get; set; }

        public string bsd_projectid { get; set; }
        public string statuscode { get; set; }
        public string bsd_address { get; set; }
        public string bsd_projecttype { get; set; }
        public string bsd_projectslogo { get; set; }
        public string logo { get {
                if (!string.IsNullOrWhiteSpace(bsd_projectslogo))
                {
                    return bsd_projectslogo;
                }
                else
                {
                    return bsd_name;
                }
            } }

        public string projectType { get { return Data.GetProjectTypeById(bsd_projecttype).Label; } }
        public string statusBackground { get { return ProjectStatusData.GetProjectStatusById(statuscode).Background; } }
        public string statusName { get { return ProjectStatusData.GetProjectStatusById(statuscode).Name; } }
    }
}
