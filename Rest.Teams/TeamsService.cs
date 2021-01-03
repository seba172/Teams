using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Rest.Teams
{
    public class TeamsService
    {
        public string CrearMeeting(string token)
        {
            var urlMeeting = "https://graph.microsoft.com/v1.0/me/onlineMeetings";
            
            return PostMeeting(token, urlMeeting);
        }

        private static string PostMeeting(string token, string urlMeeting)
        {
            string urlMeet;
            
            string myJson = "{'subject': 'Meeting Subject', 'lobbyBypassSettings':{'scope':'everyone'} }";
            
            using (var client2 = new HttpClient())
            {
                client2.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client2.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                var responseMeeting = client2.PostAsync(
                    urlMeeting,
                     new StringContent(myJson, Encoding.UTF8, "application/json")).Result;
                var result = responseMeeting.Content.ReadAsStringAsync().Result;

                dynamic jsonObject = JsonConvert.DeserializeObject<ExpandoObject>(result, new ExpandoObjectConverter()); ;
                urlMeet = jsonObject.joinWebUrl;
            };
            return urlMeet;
        }
    }
}
