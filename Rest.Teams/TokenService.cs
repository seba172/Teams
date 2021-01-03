using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Text;

namespace Rest.Teams
{
    public class TokenService
    {
        public string obtenerToken()
        {
            var urlToken = "https://login.microsoftonline.com/20f9a21d-88c0-4e21-8617-3c7298c5874a/oauth2/v2.0/token";
            var dict = new Dictionary<string, string>();
            dict.Add("grant_type", "password");
            dict.Add("userName", "amartin@developar.onmicrosoft.com");
            dict.Add("password", "Alej1211");
            dict.Add("client_secret", "uc5kaha4_8yA~_u4w.P.PKQ86aH_msybiT");
            dict.Add("client_id", "3afcdd76-99eb-446d-952c-5ee6c1142698");
            dict.Add("scope", "https://graph.microsoft.com/.default");
            var client1 = new HttpClient();
            var req = new HttpRequestMessage(HttpMethod.Post, urlToken) { Content = new FormUrlEncodedContent(dict) };
            var responseToken = client1.SendAsync(req).Result;

            string result = responseToken.Content.ReadAsStringAsync().Result;
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //dynamic jsonObject = serializer.Deserialize<dynamic>(result);
            dynamic jsonObject = JsonConvert.DeserializeObject<ExpandoObject>(result, new ExpandoObjectConverter());
            dynamic access_token = jsonObject.access_token; // result is Dictionary<string,object> user with fields name, teamname, email and players with their values

            return access_token;
        }
    }
}
