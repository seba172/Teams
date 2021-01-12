using System;
using System.Collections.Generic;
using System.Text;

namespace Rest.Teams
{
    public class ConfiguracionService
    {
        public Dictionary<string, string> ObtenerConfiguracion()
        {
            var config = new Dictionary<string, string>();
            config.Add("grant_type", "password");
            config.Add("userName", "amartin@developar.onmicrosoft.com");
            config.Add("password", "Alej1211");
            config.Add("client_secret", "uc5kaha4_8yA~_u4w.P.PKQ86aH_msybiT");
            config.Add("client_id", "3afcdd76-99eb-446d-952c-5ee6c1142698");
            config.Add("scope", "https://graph.microsoft.com/.default");
            config.Add("tenantId", "20f9a21d-88c0-4e21-8617-3c7298c5874a");

            config.Add("teamId", "205df3c8-cb94-4596-a1f8-73317f2f8278");
            config.Add("channelId", "19%3a47bf61651d4d45dfa2eb14076f235f9b%40thread.tacv2");

            return config;
        }
    }
}
