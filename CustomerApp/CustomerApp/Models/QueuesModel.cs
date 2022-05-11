using CustomerApp.Datas;
using CustomerApp.Helpers;
using CustomerApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerApp.Models
{
    public class QueuesModel : BaseViewModel, IComparer<QueuesModel>
    {
        public Guid opportunityid { get; set; }
        public string name { get; set; }
        public Guid customer_id { get; set; }
        public Guid project_id { get; set; }
        public string project_name { get; set; }
        public string contact_name { get; set; }
        public string account_name { get; set; }
        public int statuscode { get; set; }
        public string statuscode_format { get { return Data.GetQueuesById(statuscode.ToString())?.Name; } }
        public string statuscode_color { get { return Data.GetQueuesById(statuscode.ToString())?.Background; } }

        public string bsd_queuenumber { get; set; }
        public string customername
        {
            get { return contact_name ?? account_name ?? ""; }
        }
        public string telephone { get; set; }
        public int compare_sts
        {
            get
            {
                if (statuscode == 100000000)
                    return 0;
                else if (statuscode == 100000002)
                    return 1;
                else return 2;
            }
        }
        public decimal budgetamount { get; set; }
        public string description { get; set; }
        public Guid contact_id { get; set; }
        public Guid account_id { get; set; }

        private string _customer_name;
        public string customer_name { get => _customer_name; set { _customer_name = value; OnPropertyChanged(nameof(customer_name)); } }
        public Guid bsd_customerreferral_account_id { get; set; }
        public string bsd_customerreferral_name { get; set; }
        public Guid bsd_salesagentcompany_account_id { get; set; }
        public string bsd_salesagentcompany_name { get; set; }
        public string bsd_nameofstaffagent { get; set; }
        public Guid bsd_collaborator_contact_id { get; set; }
        public string bsd_collaborator_name { get; set; }

        public DateTime _createdon;
        public DateTime createdon { get => _createdon.AddHours(7); set { _createdon = value; OnPropertyChanged(nameof(createdon)); } } // Thời gian đặt chỗ 

        public DateTime _bsd_queuingexpired;
        public DateTime bsd_queuingexpired { get => _bsd_queuingexpired.AddHours(7); set { _bsd_queuingexpired = value; OnPropertyChanged(nameof(bsd_queuingexpired)); } } // Thời gian đặt chỗ  // Thời gian hết hạn

        public DateTime _bsd_bookingtime;
        public DateTime bsd_bookingtime { get => _bsd_bookingtime.AddHours(7); set { _bsd_bookingtime = value; OnPropertyChanged(nameof(bsd_bookingtime)); } } // Thời gian bat dau
        public Guid bsd_project_id { get; set; }
        public string bsd_project_name { get; set; } // dự án
        public decimal bsd_bookingf { get; set; }
        public Guid bsd_phaseslaunch_id { get; set; }
        public string bsd_phaseslaunch_name { get; set; }
        public Guid bsd_discountlist_id { get; set; } // lấy về kèm theo khi lấy phaselaucn mục đích dùng để đưa qua đặt cọc.
        public Guid bsd_block_id { get; set; }
        public string bsd_block_name { get; set; }
        public Guid bsd_floor_id { get; set; }
        public string bsd_floor_name { get; set; }
        public Guid bsd_units_id { get; set; }
        public string _bsd_units_name;
        public string bsd_units_name { get => _bsd_units_name; set { _bsd_units_name = value; OnPropertyChanged(nameof(bsd_units_name)); } }
        public decimal bsd_units_queuingfee { get; set; }
        public Guid pricelist_id { get; set; }
        public string pricelist_name { get; set; }
        public decimal constructionarea { get; set; } // diện tích xây dựng , tên gốc bsd_constructionarea => đổi lại tránh trùng khi trong form update khi lấy thông tin về.
        public decimal netsaleablearea { get; set; } // diện tích sử dụng  , tên gốc bsd_netsaleablearea => đổi lại tránh trùng khi trong form update khi lấy thông tin về.
        public bool bsd_collectedqueuingfee { get; set; } // Đã nhận tiền

        private decimal _bsd_queuingfee;
        public decimal bsd_queuingfee { get => _bsd_queuingfee; set { _bsd_queuingfee = value; OnPropertyChanged(nameof(bsd_queuingfee)); } } // phí đặt chỗ

        private string _bsd_queuingfee_format;
        public string bsd_queuingfee_format { get => _bsd_queuingfee_format; set { _bsd_queuingfee_format = value; OnPropertyChanged(nameof(bsd_queuingfee_format)); } }
        public decimal landvalue { get; set; } // giá trị đất
        public decimal unit_price { get; set; } // Giá bán , tên gốc price => đổi lại tránh trùng khi trong form update khi lấy thông tin về.
        public int bsd_longtime { get; set; }
        public int bsd_shorttime { get; set; }
        public DateTime _queue_createdon { get; set; }
        public DateTime _queue_bsd_queuingexpired { get; set; }
        public DateTime _queue_bsd_bookingtime { get; set; }
        public int UnitStatusCode { get; set; }
        public string bsd_bookingid { get; set; }
        public string _defaultuomid_value { get; set; }
        public string _transactioncurrencyid_value { get; set; }
        public decimal bsd_taxpercent { get; set; }

        private int _bsd_ordernumber;
        public int bsd_ordernumber { get => _bsd_ordernumber; set { _bsd_ordernumber = value; OnPropertyChanged(nameof(bsd_ordernumber)); } }

        private int _bsd_priorityqueue;
        public int bsd_priorityqueue { get => _bsd_priorityqueue; set { _bsd_priorityqueue = value; OnPropertyChanged(nameof(bsd_priorityqueue)); } }

        private int _bsd_prioritynumber;
        public int bsd_prioritynumber { get => _bsd_prioritynumber; set { _bsd_prioritynumber = value; OnPropertyChanged(nameof(bsd_prioritynumber)); } }

        private DateTime _bsd_dateorder;
        public DateTime bsd_dateorder { get => _bsd_dateorder; set { _bsd_dateorder = value; OnPropertyChanged(nameof(bsd_dateorder)); } }
        private bool _bsd_expired;
        public bool bsd_expired { get => _bsd_expired; set { _bsd_expired = value; OnPropertyChanged(nameof(bsd_expired)); } }
        public string bsd_expired_format { get { return Data.GetStringByBool(bsd_expired); } }
        public Guid _bsd_units_value { get; set; }
        public string unit_name { get; set; }
        public Guid _bsd_project_value { get; set; }
        public string PhoneContact { get; set; }
        public string PhoneAccount { get; set; }
        public Guid _bsd_salesagentcompany_value { get; set; }
        public string salesagentcompany_name { get; set; }
        public Guid _bsd_phaselaunch_value { get; set; }
        public string phaselaunch_name { get; set; }
        public Guid collaborator_id { get; set; }
        public string collaborator_name { get; set; }
        public Guid customerreferral_id { get; set; }
        public string customerreferral_name { get; set; }
        //bsd_queuingfeepaid
        public decimal bsd_queuingfeepaid { get; set; }
        public string bsd_queuingfeepaid_format { get => StringFormatHelper.FormatCurrency(bsd_queuingfeepaid); }
        public bool bsd_expired_icon { get => !bsd_expired; }
        public int Compare(QueuesModel x, QueuesModel y)
        {
            if (x == null)
                return -1;
            if (y == null)
                return 1;
            // check sts
            if (x.compare_sts < y.compare_sts)
                return 1;
            else if (x.compare_sts > y.compare_sts)
                return -1;
            else
            {// check bookingtime if sts = sts
                if (x.bsd_bookingtime > y.bsd_bookingtime)
                    return 1;
                else if (x.bsd_bookingtime < y.bsd_bookingtime)
                    return -1;
                else
                    return 0;
            }
        }
    }
}
