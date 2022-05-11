using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CustomerApp.Config;

namespace CustomerApp.Helper
{
    public class LoginHelper
    {
        public static async Task<HttpResponseMessage> Login()
        {
            var client = BsdHttpClient.Instance();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://login.microsoftonline.com/87bbdb08-48ba-4dbf-9c53-92ceae16c353/oauth2/token");//OrgConfig.LinkLogin
            var formContent = new FormUrlEncodedContent(new[]
                {
                        new KeyValuePair<string, string>("resource", OrgConfig.Resource),
                        new KeyValuePair<string, string>("client_secret", OrgConfig.ClientSecret),
                        new KeyValuePair<string, string>("grant_type", "client_credentials"),
                        new KeyValuePair<string, string>("client_id", OrgConfig.ClientId),
                        //new KeyValuePair<string, string>("username", OrgConfig.UserName),
                        //new KeyValuePair<string, string>("password", OrgConfig.Password),
                        //new KeyValuePair<string, string>("grant_type", "password"),

                    });
            request.Content = formContent;
            var response = await client.SendAsync(request);
            return response;
        }
    }
}
