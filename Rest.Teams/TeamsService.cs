using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
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
            
            return CrearMeeting(token, urlMeeting);
        }

        private static string CrearMeeting(string token, string urlMeeting)
        {
            string myJson = "{'subject': 'Meeting Subject', 'lobbyBypassSettings':{'scope':'everyone'} }";
            
            using (var clientMeeting = new HttpClient())
            {
                clientMeeting.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                clientMeeting.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                var responseMeeting = clientMeeting.PostAsync(urlMeeting, new StringContent(myJson, Encoding.UTF8, "application/json")).Result;

                if (responseMeeting.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    var result = responseMeeting.Content.ReadAsStringAsync().Result;

                    dynamic jsonObject = JsonConvert.DeserializeObject<ExpandoObject>(result, new ExpandoObjectConverter()); ;
                    return jsonObject.joinWebUrl;
                }
                else
                {
                    throw new Exception("No pudo generarse la llamada. Por favor intente mas tarde.");
                }
            };
        }
    }
}
