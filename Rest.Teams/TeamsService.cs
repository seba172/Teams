﻿using Domain.Teams;
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
        /*
         Valor de ejemplo
         https://teams.microsoft.com/l/channel/19%3a47bf61651d4d45dfa2eb14076f235f9b%40thread.tacv2/Notificaciones?groupId=205df3c8-cb94-4596-a1f8-73317f2f8278&tenantId=20f9a21d-88c0-4e21-8617-3c7298c5874a
         Notas para desarrollador> Actualizar los valores de teamId y channelId con el informado por la configuracion del Team
            El valor groupId se corresponde con el teamId
            El valor que se encuentra luego de channel se corresponde con el channelId
        */

        private string teamId = "4a899aa9-7a66-4236-8c14-b34289978e9d";
        private string channelId = "19%3ac3e849911bc04c15bb43c1ad3f40a0ec%40thread.tacv2";
        private string token;
        private string urlPostMeeting = Constantes.UrlApiGraph + Constantes.EndPointMeeting;
        private string urlPostNotificacionCanal;

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
                body = new { contentType = "html", content = $"Un cliente necesita contactarse: <a href={url}>Unirse a la reunion</a>" }
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

                urlPostNotificacionCanal = Constantes.UrlApiGraph + Constantes.EndPointNotificacionTeams + teamId + Constantes.EndPointNotificacionChannel + channelId + Constantes.EndPointNotificacionMessages;
                
                var responseMeeting = EnviarSolicitud(urlPostNotificacionCanal, jsonRequest, clientMeeting);

                return ProcesarRespuestaNotificacionCanal(responseMeeting, linkMeeting);
            };
        }

        private static string ProcesarRespuestaNotificacionCanal(HttpResponseMessage responseMeeting, string linkMeeting)
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
