using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerApp.Models
{
    public class OptionSetFilter : OptionSet
    {
        public string SDT { get; set; }
        public string CMND { get; set; }
        public string CCCD { get; set; }
        public string HC { get; set; }
        public string SoGPKD { get; set; }
    }
}
