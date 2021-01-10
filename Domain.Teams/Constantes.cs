using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Teams
{
    public class Constantes
    {
        public const string UrlToken = "https://login.microsoftonline.com/"; 
        public const string EndPointToken = "/oauth2/v2.0/token";
        public const string UrlApiGraph = "https://graph.microsoft.com/";
        public const string EndPointMeeting = "v1.0/me/onlineMeetings";
        public const string EndPointNotificacion = "v1.0/teams/4a899aa9-7a66-4236-8c14-b34289978e9d/channels/19%3ac3e849911bc04c15bb43c1ad3f40a0ec%40thread.tacv2/messages";
        public const string ExcepcionGenerica = "No pudo generarse la llamada. Por favor intente mas tarde.";
        public const string SubjetMeeting = "Meeting Subject";
    }
}
