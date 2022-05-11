using System;
using System.Collections.Generic;
using System.Linq;
using CustomerApp.Models;
using CustomerApp.Resources;

namespace CustomerApp.Datas
{
    public class CaseStatusCodeData
    {
        public static List<OptionSet> CaseStatusData()
        {
            return new List<OptionSet>()
            {
                new OptionSet("1", Language.dang_xu_ly_case_sts),
                new OptionSet("2", Language.dang_cho_case_sts),
                new OptionSet("3", Language.dang_cho_thong_tin_chi_tiet_case_sts),
                new OptionSet("4", Language.nghien_cuu_case_sts),
                new OptionSet("5", Language.van_de_da_duoc_giai_quyet_case_sts),
                new OptionSet("1000", Language.cung_cap_thong_tin_case_sts),
                new OptionSet("6", Language.da_huy_case_sts),
                new OptionSet("2000", Language.hop_nhat_case_sts),
                new OptionSet("0", "")
            };
        }

        public static OptionSet GetCaseStatusCodeById(string id)
        {
            return CaseStatusData().SingleOrDefault(x => x.Val == id);
        }
    }
}
