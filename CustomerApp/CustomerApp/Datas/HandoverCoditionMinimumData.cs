using CustomerApp.Models;
using CustomerApp.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomerApp.Datas
{
    public class HandoverCoditionMinimumData
    {
        public static OptionSet GetHandoverCoditionMinimum(string Id)
        {
            return HandoverCoditionMinimums().SingleOrDefault(x => x.Val == Id);
        }
        public static List<OptionSet> HandoverCoditionMinimums()
        {
            return new List<OptionSet>()
            {
                new OptionSet("100000000",Language.ban_giao_tho),//"Bare-shell"
                new OptionSet("100000001",Language.ban_giao_hoan_thien_co_ban),//"Basic Finished"
                new OptionSet("100000002",Language.ban_giao_hoan_thien),//"Fully Finished"
                new OptionSet("100000003",Language.ban_giao_hoan_thien_mat_ngoai_va_tho_ben_trong),//"Fully Finished the outside and Bare-shell the inside"
                new OptionSet("100000004",Language.goi_ban_giao_bo_sung),//"Add On Option"
            };
        }
    }
}
