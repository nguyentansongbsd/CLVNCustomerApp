using System;
namespace CustomerApp.Models
{
    public class UserModel
    {
        public Guid contactid { get; set; }
        public string fullname { get; set; }
        public string emailaddress1 { get; set; }
        public Guid _ownerid_value { get; set; }
        public string bsd_password { get; set; }
        //avata
        public string entityimage { get; set; }
    }
}
