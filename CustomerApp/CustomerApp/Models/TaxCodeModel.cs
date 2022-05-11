﻿using CustomerApp.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerApp.Models
{
    public class TaxCodeModel
    {
        public Guid bsd_taxcodeid { get; set; }
        public string bsd_taxcode { get; set; }
        public decimal bsd_value { get; set; }
        public string bsd_value_format { get => StringFormatHelper.FormatPercent(bsd_value); }
    }
}
