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

        public string CrearMeeting(string cliente, string producto)
        {
            token = new TokenService().ObtenerToken();

            string jsonRequest = ArmarBodyMeeting();

            using (var clientMeeting = new HttpClient())
            {
                CompletarHeader(token, clientMeeting);

                var responseMeeting = EnviarSolicitud(urlPostMeeting, jsonRequest, clientMeeting);

                var linkMeeting = ProcesarRespuestaMeeting(responseMeeting);

                return EnviarNotificacionACanal(linkMeeting, cliente, producto);
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

        private string ArmarBodyNotificacionCanal(string url, string cliente, string producto)
        {
            var meetingRequest = new
            {
                body = new { contentType = "html", content = $"El cliente: "+cliente+" necesita contactarse por el Producto:"+producto+" : <a href={url}>Unirse a la reunion</a>" }
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

        private string EnviarNotificacionACanal(string linkMeeting, string cliente, string producto)
        {
            string jsonRequest = ArmarBodyNotificacionCanal(linkMeeting, cliente, producto);

            using (var clientMeeting = new HttpClient())
            {
                CompletarHeader(token, clientMeeting);

                Dictionary<string, string> configuracion = new ConfiguracionService().ObtenerConfiguracion();
                var teamId = configuracion["teamId"];
                var channelId = configuracion["channelId"];
                if (configuracion["config"] == "mongo") 
                {
                    var configuracionMongo = new MongoService().ObtenerConfiguracion();
                    teamId = configuracionMongo.TeamId;
                    channelId = configuracionMongo.ChannelId;
                }
            
                //urlPostNotificacionCanal = Constantes.UrlApiGraph + Constantes.EndPointNotificacionTeams + configuracion["teamId"] + Constantes.EndPointNotificacionChannel + configuracion["channelId"] + Constantes.EndPointNotificacionMessages;
                urlPostNotificacionCanal = Constantes.UrlApiGraph + Constantes.EndPointNotificacionTeams + teamId + Constantes.EndPointNotificacionChannel + channelId + Constantes.EndPointNotificacionMessages;


                var responseMeeting = EnviarSolicitud(urlPostNotificacionCanal, jsonRequest, clientMeeting);

                return ProcesarRespuestaNotificacionCanal(responseMeeting, linkMeeting);
            };
        }

        private string ProcesarRespuestaNotificacionCanal(HttpResponseMessage responseMeeting, string linkMeeting)
        {
            if (responseMeeting.StatusCode == System.Net.HttpStatusCode.Created)
            {
                //return "Reunion Creada y Notificada: " + linkMeeting;
                return linkMeeting;
            }
            else
            {
                throw new Exception(responseMeeting.Content.ReadAsStringAsync().Result);
            }
        }
    }
}
