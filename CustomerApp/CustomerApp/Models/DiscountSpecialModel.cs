using CustomerApp.Helpers;
using CustomerApp.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomerApp.Models
{
    public class DiscountSpecialModel
    {
        public Guid bsd_discountspecialid { get; set; }
        public string bsd_name { get; set; }
        public decimal bsd_percentdiscount { get; set; }
        public decimal bsd_totalamount { get; set; }
        public string totalamount_format { get { return StringFormatHelper.FormatCurrency(bsd_totalamount) + " đ"; } }
        public string percentdiscount_format { get { return StringFormatHelper.FormatPercent(bsd_percentdiscount) + "%"; } }
        public string statuscode { get; set; }
        public string statuscode_format { get { return statuscode != string.Empty ? DiscountSpecialStatus.GetDiscountSpecialStatusById(statuscode)?.Name : null; } }
        public string statuscode_color { get { return statuscode != string.Empty ? DiscountSpecialStatus.GetDiscountSpecialStatusById(statuscode)?.Background : "#f1f1f1"; } }
    }
    public class DiscountSpecialStatus
    {
        public static List<StatusCodeModel> DiscountSpecialStatusData()
        {
            return new List<StatusCodeModel>()
            {
                new StatusCodeModel("1",Language.nhap_sts_ckdb,"#06CF79"),//Active
                new StatusCodeModel("100000000",Language.duyet_sts_ckdb,"#03ACF5"),//Approved
                new StatusCodeModel("100000001",Language.tu_choi_sts_ckdb,"#FDC206"),//Reject
                new StatusCodeModel("100000002",Language.huy,"#03ACF5"),//Canceled
                new StatusCodeModel("2",Language.vo_hieu_luc_sts_ckdb,"#FDC206"),//Inactive
                new StatusCodeModel("0","","#f1f1f1")
            };
        }

        public static StatusCodeModel GetDiscountSpecialStatusById(string id)
        {
            return DiscountSpecialStatusData().SingleOrDefault(x => x.Id == id);
        }
    }
}
