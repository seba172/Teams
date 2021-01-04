using Domain.Teams;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Text;

namespace Rest.Teams
{
    public static class TokenService
    {
        public static string ObtenerToken()
        {
            Dictionary<string, string> configuracion = ObtenerConfiguracion();
            
            var tenantId = configuracion["tenantId"];

            var urlToken = Constantes.UrlToken + tenantId + Constantes.EndPointToken;

            return GenerarToken(configuracion, urlToken);
        }

        private static dynamic GenerarToken(Dictionary<string, string> configuracion, string urlToken)
        {
            var clientToken = new HttpClient();

            var request = ArmarRequest(configuracion, urlToken);
            
            var responseToken = EnviarSolicitud(clientToken, request);

            return ProcesarRespuesta(responseToken);
        }

        private static HttpRequestMessage ArmarRequest(Dictionary<string, string> configuracion, string urlToken)
        {
            return new HttpRequestMessage(HttpMethod.Post, urlToken) { Content = new FormUrlEncodedContent(configuracion) };
        }

        private static HttpResponseMessage EnviarSolicitud(HttpClient clientToken, HttpRequestMessage request)
        {
            return clientToken.SendAsync(request).Result;
        }

        private static dynamic ProcesarRespuesta(HttpResponseMessage responseToken)
        {
            if (responseToken.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string result = responseToken.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject<ExpandoObject>(result, new ExpandoObjectConverter());
                return jsonObject.access_token;
            }
            else
            {
                throw new Exception(responseToken.Content.ReadAsStringAsync().Result);
            }
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
            config.Add("tenantId", "20f9a21d-88c0-4e21-8617-3c7298c5874a");

            return config;
        }       
    }
}
