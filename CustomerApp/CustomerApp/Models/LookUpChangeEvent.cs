using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerApp.Models
{
    public class LookUpChangeEvent : EventArgs
    {
        public object Item { get; set; }
    }
}
