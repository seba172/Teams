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
            Dictionary<string, string> configuracion = ConfiguracionService.ObtenerConfiguracion();
            
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
    }
}
