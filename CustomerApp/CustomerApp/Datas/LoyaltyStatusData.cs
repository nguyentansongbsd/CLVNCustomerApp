using System;
using System.Collections.Generic;
using System.Linq;
using CustomerApp.Models;

namespace CustomerApp.Datas
{
    public class LoyaltyStatusData
    {
        public static List<OptionModel> LoyaltyData()
        {
            return new List<OptionModel>()
            {
                new OptionModel("100000000","None"),
                new OptionModel("100000001","Active"),
            };
        }

        public static OptionModel GetLoyaltyById(string id)
        {
            return LoyaltyData().SingleOrDefault(x=>x.Id == id);
        }
    }
}
