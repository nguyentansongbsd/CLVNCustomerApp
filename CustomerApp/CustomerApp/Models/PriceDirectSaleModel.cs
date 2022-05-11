using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerApp.Models
{
    public class PriceDirectSaleModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public PriceDirectSaleModel(string id, string name, string from = null, string to = null)
        {
            Id = id;
            Name = name;
            From = from;
            To = to;
        }
    }
}
