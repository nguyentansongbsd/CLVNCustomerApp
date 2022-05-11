using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerApp.Models
{
    public class SharePonitModel
    {
        public int documentid { get; set; }
        public string sharepointdocumentid { get; set; }
        public string absoluteurl { get; set; }
        public string fullname { get; set; }
        public string filetype { get; set; }
        public string relativelocation { get; set; }
        public string author { get; set; }
    }
}
