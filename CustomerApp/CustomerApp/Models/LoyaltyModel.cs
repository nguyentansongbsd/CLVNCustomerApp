using CustomerApp.Datas;
using System;
namespace CustomerApp.Models
{
    public class LoyaltyModel
    {
        public Guid contactid { get; set; }
        public decimal? bsd_totalamountofownership3years { get; set; }
        public decimal? bsd_totalamountofownership { get; set; }
        public string bsd_loyaltystatus { get; set; }
        public DateTime? bsd_loyaltydate { get; set; }
        public Guid membershiptier_id { get; set; }
        public string membershiptier_name { get; set; }
        //Transacted da_giao_dich Đã giao dịch
        public bool bsd_transacted { get; set; }
        public string bsd_transacted_format { get { return Data.GetStringByBool(bsd_transacted); } }
        //Total Transaction tong_tien_giao_dich Tổng điểm giao dịch
        public decimal bsd_totaltransaction { get; set; }
        //Membership Tier bac_thanh_vien Bậc thành viên
        public string bsd_membershiptier { get; set; }
        public string bsd_membershiptier_name { get; set; }
        //Total Points of Ownership tong_diem_so_huu Tổng điểm sở hữu
        public decimal bsd_totalpointsofownership { get; set; }
        //Total Points (Include Condition) tong_diem_bao_gom_dieu_kien Tổng điểm bao gồm điều kiện
        public decimal bsd_totalpointsincludecondition { get; set; }
        //Total Amount in effective time tong_tien_trong_thoi_gian_hieu_luc Tổng tiền trong thời gian hiệu lực
        public decimal bsd_totalamountineffectivetime { get; set; }
        //Rank Up hang Hạng
        public string bsd_rankup { get; set; }
        public string bsd_rankup_name { get; set; }
        //Rank Up (Special) hang_dac_biet Hạng (Dặc biệt)
        public string bsd_rankupspecial { get; set; }
        public string bsd_rankupspecial_name { get; set; }
        //Total Points Of Condition tong_diem_dieu_kien Tổng điểm điều kiện
        public decimal bsd_totalpointsofcondition { get; set; }
    }
}
