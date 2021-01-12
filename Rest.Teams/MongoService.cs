using Domain.Teams;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rest.Teams
{
    public class MongoService
    {
        public static string conectionString = "mongodb://localhost:27017";
        public static string mongo_db = "prueba";
        public string ValidarPublicKey(string public_key) 
        {
            var client = new MongoClient(conectionString);
            var database = client.GetDatabase(mongo_db);
            var link = database.GetCollection<Link>("link");
            var expira = link.Find(a => a.Public_key == public_key).SingleOrDefault();


            if (expira != null)
            {
                if (expira.ExpirationDate > DateTime.Now)
                {
                    return "ok";
                }
                else
                {
                    return "Public key expired";
                }
            }
            else
            {
                return "Public key expired";
            }
        }


        /* 
Notas para desarrollador> Actualizar los valores de teamId y channelId con el informado por la configuracion del Team
    El valor groupId se corresponde con el teamId
    El valor que se encuentra luego de channel se corresponde con el channelId

    Valor de ejemplo configurado
    https://teams.microsoft.com/l/channel/19%3a47bf61651d4d45dfa2eb14076f235f9b%40thread.tacv2/Notificaciones?groupId=205df3c8-cb94-4596-a1f8-73317f2f8278&tenantId=20f9a21d-88c0-4e21-8617-3c7298c5874a
*/

        public ConfiTeams ObtenerConfiguracion()
        {
            var client = new MongoClient(conectionString);
            var database = client.GetDatabase(mongo_db);
            var configuracion = database.GetCollection<ConfiTeams>("confi_teams");
            var config = configuracion.Find(a => a.Client_Id != "").SingleOrDefault();

            return config;
        }

            // {
            //    "_id": {
            //        "$oid": "5ff3c475d0827812b0108e0e"
            //    },
            //    "client_id": "",
            //    "client_secret": "",
            //    "userName": "amartin@developar.onmicrosoft.com",
            //    "password": "Alej1211",
            //    "scope": "https://graph.microsoft.com/.default",
            //    "grant_type": "password",
            //    "tenantId": "20f9a21d-88c0-4e21-8617-3c7298c5874a",
            //    "teamId" : "205df3c8-cb94-4596-a1f8-73317f2f8278",
            //    "channelId":"19%3a47bf61651d4d45dfa2eb14076f235f9b%40thread.tacv2"    
            //}
}
}
