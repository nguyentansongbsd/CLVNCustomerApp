using System;
namespace CustomerApp.Models
{
    public class CustomerLookUp : LookUp
    {
        public int Type { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
