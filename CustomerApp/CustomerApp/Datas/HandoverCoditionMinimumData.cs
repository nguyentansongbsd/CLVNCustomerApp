using CustomerApp.Models;
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
                new OptionSet("100000000","Bàn giao thô"),//"Bare-shell"
                new OptionSet("100000001","Bàn giao hoàn thiện cơ bản"),//"Basic Finished"
                new OptionSet("100000002","Bàn giao hoàn thiện"),//"Fully Finished"
                new OptionSet("100000003","Bàn giao hoàn thiện mặt ngoài và thô bên trong"),//"Fully Finished the outside and Bare-shell the inside"
                new OptionSet("100000004","Gói bàn giao bổ sung"),//"Add On Option"
            };
        }
    }
}
