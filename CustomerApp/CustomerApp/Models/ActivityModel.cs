using CustomerApp.Resources;
using CustomerApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerApp.Models
{
    public class ActivityModel : BaseViewModel
    {
        public Guid activityid { get; set; }
        public string subject { get; set; }
        public string accounts_bsd_name { get; set; }
        public string contact_bsd_fullname { get; set; }
        public string lead_fullname { get; set; }
        public string queue_name { get; set; }
        public string systemsetup_bsd_name { get; set; }
        public string regarding_name
        {
            get
            {
                if (activitytypecode == "appointment")
                {
                    // return null;
                    if (this.contact_bsd_fullname != null)
                    {
                        return this.contact_bsd_fullname;
                    }
                    else if (this.accounts_bsd_name != null)
                    {
                        return this.accounts_bsd_name;
                    }
                    else if (this.lead_fullname != null)
                    {
                        return this.lead_fullname;
                    }
                    else if (this.queue_name != null)
                    {
                        return this.queue_name;
                    }
                    else
                    {
                        return " ";
                    }
                }

                if (activitytypecode == "phonecall")
                {
                    if (this.callto_contact_name != null)
                    {
                        return this.callto_contact_name;
                    }
                    else if (this.callto_account_name != null)
                    {
                        return this.callto_account_name;
                    }
                    else if (this.callto_lead_name != null)
                    {
                        return this.callto_lead_name;
                    }
                    else
                    {
                        return " ";
                    }
                }
                else
                {
                    if (this.accounts_bsd_name != null)
                    {
                        return this.accounts_bsd_name;
                    }
                    else if (this.contact_bsd_fullname != null)
                    {
                        return this.contact_bsd_fullname;
                    }
                    else if (this.lead_fullname != null)
                    {
                        return this.lead_fullname;
                    }
                    else if (this.systemsetup_bsd_name != null)
                    {
                        return this.systemsetup_bsd_name;
                    }
                    else if (this.queue_name != null)
                    {
                        return this.queue_name;
                    }
                    else
                    {
                        return " ";
                    }
                }
            }
        }
        public string activitytypecode { get; set; }
        public string activitytypecode_format
        {
            get
            {
                switch (activitytypecode)
                {
                    case "task":
                        return Language.cong_viec;
                    case "phonecall":
                        return Language.cuoc_goi;
                    case "appointment":
                        return Language.cuoc_hop;
                    default:
                        return " ";
                }
            }
        }
        public int statecode { get; set; }
        public string statecode_format
        {
            get
            {
                switch (this.statecode)
                {
                    case 0:
                        return Language.activity_open_sts; // activity_open_sts  Open
                    case 1:
                        return Language.activity_completed_sts; // activity_completed_sts  Completed
                    case 2:
                        return Language.activity_cancelled_sts; // activity_canceled_sts Canceled
                    case 3:
                        return Language.activity_scheduled_sts; // activity_scheduled_sts
                    default:
                        return " ";
                }
            }
        }
        public string owners_fullname { get; set; }
        public int prioritycode { get; set; }
        public string prioritycode_format
        {
            get
            {
                switch (prioritycode)
                {
                    case 0:
                        return "Low";
                    case 1:
                        return "Normal";
                    case 2:
                        return "High";
                    default:
                        return " ";
                }
            }
        }
        public DateTime scheduledstart { get; set; }
        public DateTime scheduledstart_format
        {
            get => this.scheduledstart.ToLocalTime();
        }
        public DateTime scheduledend { get; set; }
        public DateTime scheduledend_format
        {
            get => this.scheduledend.ToLocalTime();
        }
        public DateTime createdon { get; set; }
        //public string createdon_format
        //{
        //    get => StringHelper.DateFormat(this.createdon);
        //}

        public string statecode_color
        {
            get
            {
                switch (this.statecode)
                {
                    case 0:
                        return "#06CF79"; // open
                    case 1:
                        return "#03ACF5"; //com
                    case 2:
                        return "#333333"; //can
                    case 3:
                        return "#04A388"; //sha
                    default:
                        return "#333333";
                }
            }
        }

        // sử dụng cho phonecall

        public string callto_contact_name { get; set; }
        public string callto_account_name { get; set; }
        public string callto_lead_name { get; set; }

        public string _customer;
        public string customer { get => _customer; set { _customer = value; OnPropertyChanged(nameof(customer)); } }
    }
}
