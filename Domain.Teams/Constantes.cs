using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Teams
{
    public class Constantes
    {
        public const string UrlToken = "https://login.microsoftonline.com/"; 
        public const string EndPointToken = "/oauth2/v2.0/token";
        public const string UrlMeeting = "https://graph.microsoft.com/";
        public const string EndPointMeeting = "v1.0/me/onlineMeetings";
        public const string ExcepcionGenerica = "No pudo generarse la llamada. Por favor intente mas tarde.";
        public const string SubjetMeeting = "Meeting Subject";
    }
}
