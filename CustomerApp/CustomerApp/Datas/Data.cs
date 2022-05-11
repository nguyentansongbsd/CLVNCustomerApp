using CustomerApp.Models;
using CustomerApp.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomerApp.Datas
{
    public class Data
    {
        #region Gender
        public static List<OptionSet> GenderData()
        {
            return new List<OptionSet>()
            {
                new OptionSet("1",Language.gender_nam),
                new OptionSet("2",Language.gender_nu),
                new OptionSet("100000000",Language.gender_khac),
            };
        }

        public static OptionSet GetGenderById(string id)
        {
            return GenderData().SingleOrDefault(x => x.Val == id);
        }
        #endregion
        #region NetAreaDirectSale
        public static List<NetAreaDirectSaleModel> NetAreaData()
        {
            return new List<NetAreaDirectSaleModel>()
            {
                new NetAreaDirectSaleModel("1",Language.string_under + " 60 "+Language.sqm,"60"),
                new NetAreaDirectSaleModel("2","60 "+Language.sqm + " -> 80 "+Language.sqm,"60","80"),
                new NetAreaDirectSaleModel("3","81 "+Language.sqm + " -> 100 "+Language.sqm,"81","100"),
                new NetAreaDirectSaleModel("4","101 "+Language.sqm + " -> 120 "+Language.sqm,"101","120"),
                new NetAreaDirectSaleModel("5","121 "+Language.sqm + " -> 150 "+Language.sqm,"121","150"),
                new NetAreaDirectSaleModel("6","151 "+Language.sqm + " -> 180 "+Language.sqm,"151","180"),
                new NetAreaDirectSaleModel("7","211 "+Language.sqm + " -> 240 "+Language.sqm,"211","240"),
                new NetAreaDirectSaleModel("8","241 "+Language.sqm + " -> 270 "+Language.sqm,"241","270"),
                new NetAreaDirectSaleModel("9","271 "+Language.sqm + " -> 300 "+Language.sqm,"271","300"),
                new NetAreaDirectSaleModel("10","301 "+Language.sqm + " -> 350 "+Language.sqm,"301","350"),
                new NetAreaDirectSaleModel("11",Language.string_more_than + " 350 "+Language.sqm,null,"350"),
            };
        }
        public static NetAreaDirectSaleModel GetNetAreaById(string Id)
        {
            return NetAreaData().SingleOrDefault(x => x.Id == Id);
        }
        #endregion
        #region Price
        public static List<PriceDirectSaleModel> PriceData()
        {
            return new List<PriceDirectSaleModel>()
            {
                new PriceDirectSaleModel("1",Language.string_under + " 1 " + Language.currency_billion,"1000000000"), //currency_billion //duoi
                new PriceDirectSaleModel("2","1 " + Language.currency_billion + " -> 2 " + Language.currency_billion,"1000000000","2000000000"),
                new PriceDirectSaleModel("3","2 " + Language.currency_billion + " -> 5 " + Language.currency_billion,"2000000000","5000000000"),
                new PriceDirectSaleModel("4","5 " + Language.currency_billion + " -> 10 " + Language.currency_billion,"5000000000","10000000000"),
                new PriceDirectSaleModel("5","10 " + Language.currency_billion + " -> 20 " + Language.currency_billion,"10000000000","20000000000"),
                new PriceDirectSaleModel("6","20 " + Language.currency_billion + " -> 50 " + Language.currency_billion,"20000000000","50000000000"),
                new PriceDirectSaleModel("7",Language.string_more_than + " 50 "+ Language.currency_billion,null,"50000000000") //hơn
                //string_under
                // string_more_than
            };
        }
        public static PriceDirectSaleModel GetPriceById(string Id)
        {
            return PriceData().SingleOrDefault(x => x.Id == Id);
        }
        #endregion
        #region Directions
        public static List<OptionSetFilter> DirectionsData()
        {
            var directions = new List<OptionSetFilter>();
            directions.Add(new OptionSetFilter { Val = "100000000", Label = Language.direction_east });//East
            directions.Add(new OptionSetFilter { Val = "100000001", Label = Language.direction_west });//West
            directions.Add(new OptionSetFilter { Val = "100000002", Label = Language.direction_south });//South
            directions.Add(new OptionSetFilter { Val = "100000003", Label = Language.direction_north });//North
            directions.Add(new OptionSetFilter { Val = "100000004", Label = Language.direction_north_west });//North West
            directions.Add(new OptionSetFilter { Val = "100000005", Label = Language.direction_north_east });//North East
            directions.Add(new OptionSetFilter { Val = "100000006", Label = Language.direction_south_west });//South West
            directions.Add(new OptionSetFilter { Val = "100000007", Label = Language.direction_south_east });//South East
            return directions;
        }
        public static OptionSet GetDiretionById(string diretionId)
        {
            var diretion = DirectionsData().Single(x => x.Val == diretionId);
            return diretion;
        }
        #endregion
        #region StatusCodeUnit
        public static StatusCodeModel GetStatusCodeById(string statusCodeId)
        {
            return StatusCodesData().SingleOrDefault(x => x.Id == statusCodeId);
        }

        public static List<StatusCodeModel> StatusCodesData()
        {
            return new List<StatusCodeModel>()
            {
                new StatusCodeModel("0",Language.unit_draft_sts,"#333333"), //Draft unit_draft_sts
                new StatusCodeModel("1",Language.unit_preparing_sts,"#FDC206"),//Preparing unit_preparing_sts
                new StatusCodeModel("100000000",Language.unit_available_sts,"#06CF79"),//Available unit_available_sts
                new StatusCodeModel("100000007",Language.unit_booking_sts,"#00CED1"), //Booking
                new StatusCodeModel("100000004",Language.unit_queuing_sts,"#03ACF5"),//Queuing unit_queuing_sts
                new StatusCodeModel("100000006",Language.unit_reserve_sts,"#04A388"),//Reserve unit_reserve_sts
                new StatusCodeModel("100000005",Language.unit_collected_sts,"#9A40AB"),//Collected unit_collected_sts
                new StatusCodeModel("100000003",Language.unit_deposited_sts,"#FA7901"),//Deposited unit_deposited_sts
                new StatusCodeModel("100000001",Language.unit_1st_installment_sts,"#808080"),//1st Installment unit_1st_installment_sts
                new StatusCodeModel("100000002",Language.unit_sold_sts,"#D42A16"),//Sold unit_sold_sts
                new StatusCodeModel("100000009",Language.unit_signedda_sts,"#A0DB8E"), //Signed D.A
                new StatusCodeModel("100000008",Language.unit_qualified_sts,"#6897BB"),  //Qualified
                new StatusCodeModel("100000010",Language.unit_option_sts,"#808080"), //Option
            };
        }
        #endregion
        #region QueuesStatusCodeData
        public static StatusCodeModel GetQueuesById(string id)
        {
            return GetQueuesData().SingleOrDefault(x => x.Id == id);
        }
        public static List<StatusCodeModel> GetQueuesByIds(string ids)
        {
            List<StatusCodeModel> listQueue = new List<StatusCodeModel>();
            string[] Ids = ids.Split(',');
            foreach (var item in Ids)
            {
                listQueue.Add(GetQueuesById(item));
            }
            return listQueue;
        }
        public static List<StatusCodeModel> GetQueuesData()
        {
            return new List<StatusCodeModel>()
            {
                new StatusCodeModel("1",Language.queue_draft_sts,"#808080"),
                new StatusCodeModel("2",Language.queue_on_hold_sts,"#808080"),
                new StatusCodeModel("3",Language.queue_won_sts,"#808080"),
                new StatusCodeModel("4",Language.queue_canceled_sts,"#808080"),
                new StatusCodeModel("5",Language.queue_out_sold_sts,"#808080"),
                new StatusCodeModel("100000000",Language.queue_queuing_sts,"#00CF79"),
                new StatusCodeModel("100000001",Language.queue_collected_queuing_fee_sts,"#808080"),
                new StatusCodeModel("100000002",Language.queue_waiting_sts,"#FDC206"),
                new StatusCodeModel("100000003",Language.queue_expired_sts,"#B3B3B3"),
                new StatusCodeModel("100000004",Language.queue_completed_sts,"#C50147"),
                new StatusCodeModel("100000008","Xác nhận hủy","#C50147"),
                new StatusCodeModel("100000009","Hủy giữ chỗ nhưng chưa hoàn tiền","#C50147"),
                new StatusCodeModel("100000010","Hủy giữ chỗ đã hoàn tiền","#C50147"),
                new StatusCodeModel("0","","#808080")
            };
        }
        #endregion
        # region GetProjectType
        public static OptionSet GetProjectTypeById(string projectType)
        {
            return ProjectTypeData().SingleOrDefault(x => x.Val == projectType);
        }
        public static List<OptionSet> ProjectTypeData()
        {
            return new List<OptionSet>()
            {
                new OptionSet("false",Language.project_residential_type), //Residential project_residential_type
                new OptionSet("true",Language.project_commercial_type), //Commercial  project_commercial_type
            };
        }
        #endregion
        #region PropertyUsageTypes
        public static OptionSet GetPropertyUsageTypeById(string Id)
        {
            return PropertyUsageTypeData().SingleOrDefault(x => x.Val == Id);
        }
        public static List<OptionSet> PropertyUsageTypeData()
        {
            return new List<OptionSet>()
            {
                new OptionSet("1",Language.project_condo_usagetype), //Condo project_condo_usagetype
                new OptionSet("2",Language.project_apartment_usagetype), //Apartment project_apartment_usagetype
                new OptionSet("3",Language.project_townhouse_usagetype), //Townhouse project_townhouse_usagetype
            };
        }
        #endregion
        #region ProjectStatusCode
        public static List<StatusCodeModel> ProjectStatusCodeData()
        {
            return new List<StatusCodeModel>()
            {
                new StatusCodeModel("1","Active","#14A184"),//Active
                new StatusCodeModel("861450002","Publish","#2FCC71"),//Publish
                new StatusCodeModel("861450001","Unpublish","#808080"),//Unpublish
                new StatusCodeModel("2","Inactive","#D90825")//Inactive
            };
        }
        public static StatusCodeModel GetProjectStatusCodeById(string id)
        {
            return ProjectStatusCodeData().Single(x => x.Id == id);
        }
        #endregion
        #region bool to string
        public static string GetStringByBool(bool _bool)
        {
            if (_bool)
            {
                return Language.co;
            }
            else
            {
                return Language.khong;
            }
        }
        #endregion
        #region Quote
        public static List<StatusCodeModel> QuoteStatusData()
        {
            return new List<StatusCodeModel>()
            {
                new StatusCodeModel("100000000",Language.quote_dat_coc,"#ffc43d"), // Reservation
                new StatusCodeModel("100000001",Language.quote_da_thanh_ly,"#F43927"), // Terminated
                new StatusCodeModel("100000002",Language.quote_dang_cho_huy_bo_tien_gui,"#808080"),//Pending Cancel Deposit
                new StatusCodeModel("100000003",Language.quote_tu_choi,"#808080"), // Reject
                new StatusCodeModel("100000004",Language.quote_da_ky_rf,"#808080"),//Signed RF
                new StatusCodeModel("100000005",Language.quote_da_het_han_ky_rf,"#808080"), // Expired of signing RF
                new StatusCodeModel("100000006",Language.quote_chuyen_coc,"#808080"),//Collected
                new StatusCodeModel("100000007",Language.quote_bao_gia,"#FF8F4F"), //Quotation
                new StatusCodeModel("100000008",Language.quote_bao_gia_het_han,"#808080"), // Expired Quotation
                new StatusCodeModel("100000009",Language.quote_het_han,"#B3B3B3"), // ~ Het han
                new StatusCodeModel("100000010",Language.quote_da_ky_phieu_coc,"#808080"),//Đã ký phiếu cọc
                new StatusCodeModel("100000012",Language.quote_nhap,"#808080"),

                new StatusCodeModel("1",Language.quote_dang_xu_ly,"#00CF79"),//In Progress
                new StatusCodeModel("2",Language.quote_dang_xu_ly,"#00CF79"),//In Progress
                new StatusCodeModel("3",Language.quote_tt_du_tien_coc,"#04A8F4"),//Deposited
                new StatusCodeModel("4",Language.quote_thanh_cong,"#8bce3d"), // Won
                new StatusCodeModel("5",Language.quote_mat_khach_hang,"#808080"), // Lost
                new StatusCodeModel("6",Language.quote_da_huy,"#808080"), // Canceled
                new StatusCodeModel("7",Language.quote_da_sua_doi,"#808080"), // Revised

                new StatusCodeModel("861450001",Language.quote_da_trinh,"#808080"),//Submitted
                new StatusCodeModel("861450002",Language.quote_da_duyet,"#808080"),//Approved
                new StatusCodeModel("861450000",Language.quote_thay_doi_thong_tin,"#808080"),//Change information
            };
        }

        public static StatusCodeModel GetQuoteStatusCodeById(string id)
        {
            return QuoteStatusData().SingleOrDefault(x => x.Id == id);
        }
        #endregion
        #region Views
        public static OptionSetFilter GetViewById(string viewId)
        {
            var view = ViewData().SingleOrDefault(x => x.Val == viewId);
            return view;
        }

        public static string GetViewByIds(string ids)
        {
            List<string> list = new List<string>();
            string[] Ids = ids.Split(',');
            foreach (var item in Ids)
            {
                var i = GetViewById(item);
                if (i != null)
                    list.Add(i.Label);
            }
            return string.Join(", ", list);
        }

        public static List<OptionSetFilter> ViewData()
        {
            return new List<OptionSetFilter>() {
                new OptionSetFilter(){ Val="100000000",Label=Language.thanh_pho},
                new OptionSetFilter(){ Val="100000001",Label=Language.be_boi },  // hồ bơi
                new OptionSetFilter(){ Val="100000002",Label=Language.cong_vien},
                new OptionSetFilter(){ Val="100000003",Label=Language.mat_tien},
                new OptionSetFilter(){ Val="100000004",Label=Language.san_vuon},
                new OptionSetFilter(){ Val="100000006",Label=Language.xa_lo},
                new OptionSetFilter(){ Val="100000007",Label=Language.ho}, //lake
                new OptionSetFilter(){ Val="100000008",Label=Language.song},
                new OptionSetFilter(){ Val="100000009",Label=Language.bien},
                new OptionSetFilter(){ Val="100000010",Label=Language.mot_mat_thoang},
                new OptionSetFilter(){ Val="100000011",Label=Language.hai_mat_thoang},
                new OptionSetFilter(){ Val="100000012",Label=Language.ho_boi}, // pool
            };
        }
        #endregion
        #region PhasesLanch
        public static StatusCodeModel GetPhasesLanchStatusCodeById(string statusCodeId)
        {
            return PhasesLanchStatusCodeData().SingleOrDefault(x => x.Id == statusCodeId);
        }

        public static List<StatusCodeModel> PhasesLanchStatusCodeData()
        {
            return new List<StatusCodeModel>()
            {
                new StatusCodeModel("1",Language.phaseslanch_not_launch_sts,"#FDC206"), //Not Launch
                new StatusCodeModel("100000000",Language.phaseslanch_launched_sts,"#06CF79"),//Launched
                new StatusCodeModel("100000004",Language.phaseslanch_approved_sts,"#03ACF5"),//Approved
                new StatusCodeModel("100000003",Language.phaseslanch_confirm_sts,"#FA7901"),//Confirm
                new StatusCodeModel("100000001",Language.phaseslanch_recovery_sts,"#808080"),//Recovery
                new StatusCodeModel("100000002",Language.phaseslanch_unit_recovery_sts,"#D42A16"),//Unit Recovery
            };
        }
        #endregion
        #region PhasesLanch Form of distribution
        public static StatusCodeModel GetPhasesLanchFODById(string statusCodeId)
        {
            return PhasesLanchFODData().SingleOrDefault(x => x.Id == statusCodeId);
        }

        public static List<StatusCodeModel> PhasesLanchFODData()
        {
            return new List<StatusCodeModel>()
            {
                new StatusCodeModel("100000000",Language.phaseslanch_one_system_fod,"#06CF79"),//One system
                new StatusCodeModel("100000001",Language.phaseslanch_many_system_fod,"#808080"),//Many system
            };
        }
        #endregion
        #region PhasesLanch Release
        public static StatusCodeModel GetPhasesLanchReleaseById(string statusCodeId)
        {
            return PhasesLanchReleaseData().SingleOrDefault(x => x.Id == statusCodeId);
        }

        public static List<StatusCodeModel> PhasesLanchReleaseData()
        {
            return new List<StatusCodeModel>()
            {
                new StatusCodeModel("100000000",Language.phaseslanch_release_again_release,"#06CF79"),//Release again
                new StatusCodeModel("100000001",Language.phaseslanch_release_new_release,"#808080"),//Release new
            };
        }
        #endregion
        #region Contract Data
        public static List<StatusCodeModel> ContractStatusData()
        {
            return new List<StatusCodeModel>()
            {
                new StatusCodeModel("100000001",Language.contract_1st_installment_sts,"#F43927"), // thanh toán đợt 1 contract_1st_installment_sts //  1st Installment 
                new StatusCodeModel("100000003",Language.contract_being_payment_sts,"#F43927"), // contract_being_payment_sts //Being Payment
                new StatusCodeModel("4",Language.contract_canceled_sts,"#8bce3d"), // đã hủy  contract_canceled_sts //Canceled
                new StatusCodeModel("100001",Language.contract_complete_sts,"#FF8F4F"), // hoàn thành// contract_complete_sts //Complete
                new StatusCodeModel("100000004",Language.contract_complete_payment_sts,"#FF8F4F"), // contract_complete_payment_sts //Complete Payment
                new StatusCodeModel("100000007",Language.contract_qualify_sts,"#FF8F4F"),// contract_converted_sts //Qualify
                new StatusCodeModel("100000005",Language.contract_handover_sts,"#FF8F4F"),// contract_handover_sts //Quanlify - Units Handover
                new StatusCodeModel("3",Language.contract_in_progress_sts,"#FF8F4F"),// contract_in_progress_sts //In Progress
                new StatusCodeModel("100003",Language.contract_invoiced_sts,"#FF8F4F"),// contract_invoiced_sts //Invoiced
                new StatusCodeModel("1",Language.contract_open_sts,"#FF8F4F"),// contract_open_sts //Open
                new StatusCodeModel("100000000",Language.contract_option_sts,"#ffc43d"), // contract_option_sts //Option
                new StatusCodeModel("100002",Language.contract_partial_sts,"#FF8F4F"),// contract_partial_sts //Partial
                new StatusCodeModel("2",Language.contract_pending_sts,"#FF8F4F"),// contract_pending_sts //Pending
                new StatusCodeModel("100000002",Language.contract_signed_contract_sts,"#FF8F4F"), // đã ký hợp đồng // contract_signed_contract_sts //Signed Contract
                new StatusCodeModel("100000006",Language.contract_terminated_sts,"#c4c4c4"), // đã thanh lý // contract_terminated_sts //Terminated
                new StatusCodeModel("0","","#bfbfbf"),
                new StatusCodeModel("100000008",Language.contract_signed_da_sts,"#c4c4c4"), // ???? // contract_signed_da_sts //Signed D.A
                new StatusCodeModel("100000009",Language.contract_acceptance_sts,"#0000ff"), // ???? // contract_acceptance_sts //Acceptance
                new StatusCodeModel("100000010",Language.contract_units_handover_sts,"#0000ff"), // ???? // contract_units_handover_sts //Units Handover
                new StatusCodeModel("100000011",Language.contract_confirm_document_sts,"#0000ff"), // ???? // contract_confirm_document_sts //Confirm Document
                new StatusCodeModel("100000012",Language.contract_pink_book_handover_sts,"#0000ff"), // ???? // contract_pink_book_handover_sts //Pink-book Handover
            };
        }

        public static StatusCodeModel GetContractStatusCodeById(string id)
        {
            return ContractStatusData().SingleOrDefault(x => x.Id == id);
        }
        #endregion
    }
}
