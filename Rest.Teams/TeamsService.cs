using Domain.Teams;
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
        private string token;
        private string urlPostMeeting = Constantes.UrlApiGraph + Constantes.EndPointMeeting;
        private string urlPostNotificacionCanal = Constantes.UrlApiGraph + Constantes.EndPointNotificacion;

        public string CrearMeeting()
        {
            token = TokenService.ObtenerToken();


            string jsonRequest = ArmarBodyMeeting();

            using (var clientMeeting = new HttpClient())
            {
                CompletarHeader(token, clientMeeting);

                var responseMeeting = EnviarSolicitud(urlPostMeeting, jsonRequest, clientMeeting);

                var linkMeeting = ProcesarRespuestaMeeting(responseMeeting);

                return EnviarNotificacionACanal(linkMeeting);
            };
        }       

        private static string ArmarBodyMeeting()
        {
            var meetingRequest = new
            {
                subject = Constantes.SubjetMeeting,
                lobbyBypassSettings = new { scope = "everyone" }
            };

            string jsonRequest = JsonConvert.SerializeObject(meetingRequest);
            return jsonRequest;
        }

        private static string ArmarBodyNotificacionCanal(string url)
        {
            var meetingRequest = new
            {
                body = new { content = url }
            };

            string jsonRequest = JsonConvert.SerializeObject(meetingRequest);
            return jsonRequest;
        }

        private static void CompletarHeader(string token, HttpClient clientMeeting)
        {
            clientMeeting.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            clientMeeting.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
        }

        private static HttpResponseMessage EnviarSolicitud(string urlMeeting, string jsonRequest, HttpClient clientMeeting)
        {
            return clientMeeting.PostAsync(urlMeeting, new StringContent(jsonRequest, Encoding.UTF8, "application/json")).Result;
        }

        private static string ProcesarRespuestaMeeting(HttpResponseMessage responseMeeting)
        {
            if (responseMeeting.StatusCode == System.Net.HttpStatusCode.Created)
            {
                var result = responseMeeting.Content.ReadAsStringAsync().Result;

                dynamic jsonObject = JsonConvert.DeserializeObject<ExpandoObject>(result, new ExpandoObjectConverter());
                return jsonObject.joinWebUrl;
            }
            else
            {
                throw new Exception(responseMeeting.Content.ReadAsStringAsync().Result);
            }
        }

        private string EnviarNotificacionACanal(string linkMeeting)
        {
            string jsonRequest = ArmarBodyNotificacionCanal(linkMeeting);

            using (var clientMeeting = new HttpClient())
            {
                CompletarHeader(token, clientMeeting);

                var responseMeeting = EnviarSolicitud(urlPostNotificacionCanal, jsonRequest, clientMeeting);

                return ProcesarRespuestaNotificacionCanal(responseMeeting);
            };
        }

        private static string ProcesarRespuestaNotificacionCanal(HttpResponseMessage responseMeeting)
        {
            if (responseMeeting.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return "Reunion Creada y Notificada";
            }
            else
            {
                throw new Exception(responseMeeting.Content.ReadAsStringAsync().Result);
            }
        }
    }
}
