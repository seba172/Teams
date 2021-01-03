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
        public string ObtenerToken()
        {
            var urlToken = "https://login.microsoftonline.com/20f9a21d-88c0-4e21-8617-3c7298c5874a/oauth2/v2.0/token";

            Dictionary<string, string> configuracion = ObtenerConfiguracion();

            return PostToken(configuracion, urlToken);
        }

        private static dynamic PostToken(Dictionary<string, string> configuracion, string urlToken)
        {
            var client1 = new HttpClient();
            var req = new HttpRequestMessage(HttpMethod.Post, urlToken) { Content = new FormUrlEncodedContent(configuracion) };
            var responseToken = client1.SendAsync(req).Result;

            string result = responseToken.Content.ReadAsStringAsync().Result;
            dynamic jsonObject = JsonConvert.DeserializeObject<ExpandoObject>(result, new ExpandoObjectConverter());
            dynamic access_token = jsonObject.access_token;
            return access_token;
        }

        private static Dictionary<string, string> ObtenerConfiguracion()
        {
            var config = new Dictionary<string, string>();
            config.Add("grant_type", "password");
            config.Add("userName", "amartin@developar.onmicrosoft.com");
            config.Add("password", "Alej1211");
            config.Add("client_secret", "uc5kaha4_8yA~_u4w.P.PKQ86aH_msybiT");
            config.Add("client_id", "3afcdd76-99eb-446d-952c-5ee6c1142698");
            config.Add("scope", "https://graph.microsoft.com/.default");
            return config;
        }
    }
}
