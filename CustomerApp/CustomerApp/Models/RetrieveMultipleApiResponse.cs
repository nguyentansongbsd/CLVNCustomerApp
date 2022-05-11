using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerApp.Models
{
    public class RetrieveMultipleApiResponse<T> where T : class
    {
        public List<T> value { get; set; }
    }
}
