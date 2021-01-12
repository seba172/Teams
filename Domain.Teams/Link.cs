using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Teams
{
    public class Link
    {
        [BsonId]
        [BsonElement("_id")]
        public string Id { get; set; }
        [BsonElement("companyId")]
        public string CompanyId { get; set; }
        [BsonElement("createdAt")]
        public DateTime? CreatedAt { get; set; }
        [BsonElement("updatedAt")]
        public DateTime? UpdatedAt { get; set; }
        [BsonElement("owner")]
        public string Owner { get; set; }

        [BsonElement("cartId")]
        public string CartId { get; set; }
        [BsonElement("public_key")]
        public string Public_key { get; set; }
        [BsonElement("expirationDate")]
        public DateTime? ExpirationDate { get; set; }
    }
}
