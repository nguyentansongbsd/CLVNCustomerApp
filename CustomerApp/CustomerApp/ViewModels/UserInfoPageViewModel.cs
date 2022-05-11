using CustomerApp.Datas;
using CustomerApp.Helper;
using CustomerApp.Models;
using CustomerApp.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CustomerApp.ViewModels
{
    public class UserInfoPageViewModel : BaseViewModel
    {
        private ContactModel _contact;
        public ContactModel Contact { get => _contact; set { _contact = value; OnPropertyChanged(nameof(Contact)); } }
        public string UserName { get; set; }
        public byte[] AvatarArr { get; set; }

        private string _avatar;
        public string Avatar { get => _avatar; set { _avatar = value; OnPropertyChanged(nameof(Avatar)); } }

        private string _password;
        public string Password { get => _password; set { _password = value; OnPropertyChanged(nameof(Password)); } }

        private string _oldPassword;
        public string OldPassword { get => _oldPassword; set { _oldPassword = value; OnPropertyChanged(nameof(OldPassword)); } }

        private string _newPassword;
        public string NewPassword { get => _newPassword; set { _newPassword = value; OnPropertyChanged(nameof(NewPassword)); } }

        private string _confirmNewPassword;
        public string ConfirmNewPassword { get => _confirmNewPassword; set { _confirmNewPassword = value; OnPropertyChanged(nameof(ConfirmNewPassword)); } }

        private OptionSet _gender;
        public OptionSet Gender { get => _gender; set { _gender = value; OnPropertyChanged(nameof(Gender)); } }

        private List<OptionSet> _genders;
        public List<OptionSet> Genders { get => _genders; set { _genders = value; OnPropertyChanged(nameof(Genders)); } }

        private AddressModel _addressContact;
        public AddressModel AddressContact { get => _addressContact; set { _addressContact = value; OnPropertyChanged(nameof(AddressContact)); } }
        
        private PhongThuyModel _phongThuy;
        public PhongThuyModel PhongThuy { get => _phongThuy; set { _phongThuy = value; OnPropertyChanged(nameof(PhongThuy)); } }
        public ObservableCollection<HuongPhongThuy> list_HuongTot { set; get; }
        public ObservableCollection<HuongPhongThuy> list_HuongXau { set; get; }

        private LoyaltyModel _loyalty;
        public LoyaltyModel Loyalty { get => _loyalty; set { _loyalty = value; OnPropertyChanged(nameof(Loyalty)); } }
        public UserInfoPageViewModel()
        {
            PhongThuy = new PhongThuyModel();
            list_HuongTot = new ObservableCollection<HuongPhongThuy>();
            list_HuongXau = new ObservableCollection<HuongPhongThuy>();
            Password = UserLogged.Password;
            Genders = Data.GenderData();
            Avatar = UserLogged.Avartar;
        }
        //load thông tin contact
        public async Task LoadContact()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='contact'>
                                    <attribute name='bsd_fullname' />
                                    <attribute name='mobilephone' />
                                    <attribute name='bsd_identitycardnumber' />
                                    <attribute name='gendercode' />
                                    <attribute name='emailaddress1' />
                                    <attribute name='createdon' />
                                    <attribute name='birthdate' />
                                    <attribute name='contactid' />
                                    <attribute name='bsd_postalcode' />
                                    <attribute name='bsd_housenumberstreet' />
                                    <attribute name='bsd_contactaddress' />
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='contactid' operator='eq' value='{UserLogged.Id}'/>
                                    </filter>
                                    <link-entity name='bsd_country' from='bsd_countryid' to='bsd_country' visible='false' link-type='outer' alias='a_8b5241be19dbeb11bacb002248168cad'>
                                        <attribute name='bsd_countryid' alias='country_id'/>
                                        <attribute name='bsd_countryname' alias='country_name'/>
                                        <attribute name='bsd_nameen'  alias='country_name_en'/>
                                    </link-entity>
                                    <link-entity name='new_province' from='new_provinceid' to='bsd_province' visible='false' link-type='outer' alias='a_8fd440dc19dbeb11bacb002248168cad'>
                                        <attribute name='new_provinceid' alias='province_id'/>
                                        <attribute name='new_name' alias='province_name'/>
                                        <attribute name='bsd_nameen'  alias='province_name_en'/>
                                    </link-entity>
                                    <link-entity name='new_district' from='new_districtid' to='bsd_district' visible='false' link-type='outer' alias='a_50d440dc19dbeb11bacb002248168cad'>
                                        <attribute name='new_districtid' alias='district_id'/>
                                        <attribute name='new_name' alias='district_name'/>
                                        <attribute name='bsd_nameen'  alias='district_name_en'/>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ContactModel>>("contacts", fetchXml);
            if (result == null || result.value.Any() == false) return;

            Contact = result.value.SingleOrDefault();
            Gender = Data.GetGenderById(Contact.gendercode);
            AddressContact = new AddressModel
            {
                country_id = Contact.country_id,
                country_name = !string.IsNullOrWhiteSpace(Contact.country_name_en) && UserLogged.Language == "en" ? Contact.country_name_en : Contact.country_name,
                country_name_en = Contact.country_name_en,
                province_id = Contact.province_id,
                province_name = !string.IsNullOrWhiteSpace(Contact.province_name_en) && UserLogged.Language == "en" ? Contact.province_name_en : Contact.province_name,
                province_name_en = Contact.province_name_en,
                district_id = Contact.district_id,
                district_name = !string.IsNullOrWhiteSpace(Contact.district_name_en) && UserLogged.Language == "en" ? Contact.district_name_en : Contact.district_name,
                district_name_en = Contact.district_name_en,
                address = Contact.bsd_contactaddress,
                lineaddress = Contact.bsd_housenumberstreet
            };
            LoadPhongThuy();
        }
        // thay doi mat khau
        public async Task<bool> ChangePassword()
        {
            string path = $"/contacts({UserLogged.Id})";
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["bsd_password"] = this.ConfirmNewPassword;

            var content = data as object;
            CrmApiResponse apiResponse = await CrmHelper.PatchData(path, content);
            if (apiResponse.IsSuccess)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        // update avata
        public async Task<bool> ChangeAvatar()
        {
            string path = $"/contacts({UserLogged.Id})";
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["entityimage"] = this.AvatarArr;

            var content = data as object;
            CrmApiResponse apiResponse = await CrmHelper.PatchData(path, content);
            if (apiResponse.IsSuccess)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task<bool> UpdateUserInfor()
        {
            string path = $"/contacts({UserLogged.Id})";
            var content = await GetContent();
            CrmApiResponse apiResponse = await CrmHelper.PatchData(path, content);
            if (apiResponse.IsSuccess)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<Boolean> DeletLookup(string fieldName, Guid contactId)
        {
            var result = await CrmHelper.SetNullLookupField("contacts", contactId, fieldName);
            return result.IsSuccess;
        }

        private async Task<object> GetContent()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["emailaddress1"] = Contact.emailaddress1;
            data["mobilephone"] = Contact.mobilephone;
            data["birthdate"] = (DateTime.Parse(Contact.birthdate.ToString()).ToLocalTime()).ToString("yyyy-MM-dd");
            data["gendercode"] = this.Gender?.Val;
           // data["bsd_postalcode"] = Contact.bsd_postalcode;

            if (AddressContact != null && !string.IsNullOrWhiteSpace(AddressContact.lineaddress))
            {
                if (!string.IsNullOrWhiteSpace(AddressContact.address))
                    data["bsd_contactaddress"] = AddressContact.address;
                if (!string.IsNullOrWhiteSpace(AddressContact.lineaddress))
                    data["bsd_housenumberstreet"] = AddressContact.lineaddress;

                if (AddressContact.country_id != Guid.Empty)
                    data["bsd_country@odata.bind"] = "/bsd_countries(" + AddressContact.country_id + ")";
                else
                    await DeletLookup("bsd_country", Contact.contactid);

                if (AddressContact.province_id != Guid.Empty)
                    data["bsd_province@odata.bind"] = "/new_provinces(" + AddressContact.province_id + ")";
                else
                    await DeletLookup("bsd_province", Contact.contactid);

                if (AddressContact.district_id != Guid.Empty)
                    data["bsd_district@odata.bind"] = "/new_districts(" + AddressContact.district_id + ")";
                else
                    await DeletLookup("bsd_district", Contact.contactid);
            }

            return data;
        }
        // phong thuy
        public void LoadPhongThuy()
        {
            if (Contact.gendercode != null)
            {
                Gender = Data.GetGenderById(Contact.gendercode);
            }
            if (list_HuongTot != null || list_HuongXau != null)
            {
                list_HuongTot.Clear();
                list_HuongXau.Clear();
                if (Contact != null && Contact.gendercode != null && Gender != null)
                {
                    PhongThuy.gioi_tinh = Int32.Parse(Contact.gendercode);
                    PhongThuy.nam_sinh = Contact.birthdate.HasValue ? Contact.birthdate.Value.Year : 0;
                    if (PhongThuy.huong_tot != null && PhongThuy.huong_tot != null)
                    {
                        string[] huongtot = PhongThuy.huong_tot.Split('\n');
                        string[] huongxau = PhongThuy.huong_xau.Split('\n');
                        int i = 1;
                        foreach (var x in huongtot)
                        {
                            string[] huong = x.Split(':');
                            string name_huong = i + ". " + huong[0];
                            string detail_huong = huong[1].Remove(0, 1);
                            list_HuongTot.Add(new HuongPhongThuy { Name = name_huong, Detail = detail_huong });
                            i++;
                        }
                        int j = 1;
                        foreach (var x in huongxau)
                        {
                            string[] huong = x.Split(':');
                            string name_huong = j + ". " + huong[0];
                            string detail_huong = huong[1].Remove(0, 1);
                            list_HuongXau.Add(new HuongPhongThuy { Name = name_huong, Detail = detail_huong });
                            j++;
                        }
                    }
                }
                else
                {
                    PhongThuy.gioi_tinh = 0;
                    PhongThuy.nam_sinh = 0;
                }
            }
        }
        // loyaty
        public async Task LoadLoyalty()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='contact'>
                                    <attribute name='contactid' />
                                    <attribute name='bsd_totalamountofownership' />
                                    <attribute name='bsd_loyaltydate' />
                                    <attribute name='bsd_transacted' />
                                    <attribute name='bsd_totaltransaction' />
                                    <attribute name='bsd_membershiptier' />
                                    <attribute name='bsd_totalpointsofownership' />
                                    <attribute name='bsd_totalpointsincludecondition' />
                                    <attribute name='bsd_rankup' />
                                    <attribute name='bsd_rankupspecial' />
                                    <attribute name='bsd_totalpointsofcondition' />
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='contactid' operator='eq' value='{UserLogged.Id}'/>
                                    </filter>
                                    <link-entity name='bsd_membershiptier' from='bsd_membershiptierid' to='bsd_membershiptier' link-type='inner' alias='ad' >
                                        <attribute name='bsd_name' alias='membershiptier_name'/>
                                        <attribute name='bsd_membershiptierid' alias='membershiptier_id'/>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<LoyaltyModel>>("contacts", fetchXml);
            if (result == null && result.value == null) return;
            this.Loyalty = (result.value as List<LoyaltyModel>).SingleOrDefault();
        }
    }
}
