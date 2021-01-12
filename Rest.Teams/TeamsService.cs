using Domain.Teams;
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
        private string token;
        private string urlPostMeeting = Constantes.UrlApiGraph + Constantes.EndPointMeeting;
        private string urlPostNotificacionCanal;

        public string CrearMeeting()
        {
            token = new TokenService().ObtenerToken();

            string jsonRequest = ArmarBodyMeeting();

            using (var clientMeeting = new HttpClient())
            {
                CompletarHeader(token, clientMeeting);

                var responseMeeting = EnviarSolicitud(urlPostMeeting, jsonRequest, clientMeeting);

                var linkMeeting = ProcesarRespuestaMeeting(responseMeeting);

                return EnviarNotificacionACanal(linkMeeting);
            };
        }       

        private string ArmarBodyMeeting()
        {
            var meetingRequest = new
            {
                subject = Constantes.SubjetMeeting,
                lobbyBypassSettings = new { scope = "everyone" }
            };

            string jsonRequest = JsonConvert.SerializeObject(meetingRequest);
            return jsonRequest;
        }

        private string ArmarBodyNotificacionCanal(string url)
        {
            var meetingRequest = new
            {
                body = new { contentType = "html", content = $"Un cliente necesita contactarse: <a href={url}>Unirse a la reunion</a>" }
            };

            string jsonRequest = JsonConvert.SerializeObject(meetingRequest);
            return jsonRequest;
        }

        private void CompletarHeader(string token, HttpClient clientMeeting)
        {
            clientMeeting.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            clientMeeting.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
        }

        private HttpResponseMessage EnviarSolicitud(string urlMeeting, string jsonRequest, HttpClient clientMeeting)
        {
            return clientMeeting.PostAsync(urlMeeting, new StringContent(jsonRequest, Encoding.UTF8, "application/json")).Result;
        }

        private string ProcesarRespuestaMeeting(HttpResponseMessage responseMeeting)
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

                Dictionary<string, string> configuracion = new ConfiguracionService().ObtenerConfiguracion();

                urlPostNotificacionCanal = Constantes.UrlApiGraph + Constantes.EndPointNotificacionTeams + configuracion["teamId"] + Constantes.EndPointNotificacionChannel + configuracion["channelId"] + Constantes.EndPointNotificacionMessages;
                
                var responseMeeting = EnviarSolicitud(urlPostNotificacionCanal, jsonRequest, clientMeeting);

                return ProcesarRespuestaNotificacionCanal(responseMeeting, linkMeeting);
            };
        }

        private string ProcesarRespuestaNotificacionCanal(HttpResponseMessage responseMeeting, string linkMeeting)
        {
            if (responseMeeting.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return "Reunion Creada y Notificada: " + linkMeeting;
            }
            else
            {
                throw new Exception(responseMeeting.Content.ReadAsStringAsync().Result);
            }
        }
    }
}
