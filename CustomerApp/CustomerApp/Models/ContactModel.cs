using CustomerApp.Datas;
using CustomerApp.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerApp.Models
{
    public class ContactModel
    {
        // id
        public Guid contactid { get; set; }
        // họ tên
        public string bsd_fullname { get; set; }
        // id giới tính
        public string gendercode { get; set; }
        // giới tính format
        public string gender_format { get { return !string.IsNullOrWhiteSpace(gendercode) ? Data.GetGenderById(gendercode)?.Name : null; } }
        //email
        public string emailaddress1 { get; set; }
        // sdt
        public string mobilephone { get; set; }
        // ngày sinh
        public DateTime? birthdate { get; set; }
        // địa chỉ
        public Guid country_id { get; set; }
        public string country_name { get; set; }
        public string country_name_en { get; set; }
        public Guid province_id { get; set; }
        public string province_name { get; set; }
        public string province_name_en { get; set; }
        public Guid district_id { get; set; }
        public string district_name { get; set; }
        public string district_name_en { get; set; }
        public string bsd_contactaddress { get; set; }
        public string bsd_housenumberstreet { get; set; }
    }
}
