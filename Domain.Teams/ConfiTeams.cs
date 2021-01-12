using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Teams
{
    public class ConfiTeams
    {
        [BsonId]
        [BsonElement("_id")]
        public ObjectId Id { get; set; }
        [BsonElement("client_id")]
        public string Client_Id { get; set; }
        [BsonElement("client_secret")]
        public string Client_Secret { get; set; }
        [BsonElement("userName")]
        public string UserName { get; set; }
        [BsonElement("password")]
        public string Password { get; set; }
        [BsonElement("scope")]
        public string Scope { get; set; }
        [BsonElement("grant_type")]
        public string Grant_Type { get; set; }
        [BsonElement("tenantId")]
        public string TenantId { get; set; }
        //Configuraciones para generar notificaciones a channels
        [BsonElement("teamId")]
        public string TeamId { get; set; }
        [BsonElement("channelId")]
        public string ChannelId { get; set; }


    }
}
