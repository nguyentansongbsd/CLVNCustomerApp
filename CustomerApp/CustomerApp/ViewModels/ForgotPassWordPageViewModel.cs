using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerApp.Helper;
using CustomerApp.Models;

namespace CustomerApp.ViewModels
{
    public class ForgotPassWordPageViewModel :BaseViewModel
    {
        private string _userName;
        public string UserName { get => _userName; set { _userName = value;OnPropertyChanged(nameof(UserName)); } }

        private string _phone;
        public string Phone { get => _phone; set { _phone = value; OnPropertyChanged(nameof(Phone)); } }

        private string _newPassword;
        public string NewPassword { get => _newPassword; set { _newPassword = value; OnPropertyChanged(nameof(NewPassword)); } }

        private string _confiemPassword;
        public string ConfirmPassword { get => _confiemPassword; set { _confiemPassword = value; OnPropertyChanged(nameof(ConfirmPassword)); } }

        public UserModel User { get; set; }

        public ForgotPassWordPageViewModel()
        {

        }

        public async Task CheckUserName()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                  <entity name='contact'>
                    <attribute name='contactid' />
                    <attribute name='fullname' />
                    <filter type='or'>
                      <condition attribute='fullname' operator='eq' value='{UserName}' />
                      <condition attribute='emailaddress1' operator='eq' value='{UserName}' />
                    </filter>
                  </entity>
                </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<UserModel>>("contacts", fetchXml);
            if (result == null || result.value.Count == 0)
                User = null;
            else
                User = (result.value as List<UserModel>).SingleOrDefault();
        }
    }
}
