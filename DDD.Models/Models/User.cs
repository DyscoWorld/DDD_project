using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;

namespace DDD.Models.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string TelegramId { get; set; }
        public string Username { get; set; }
        public ICollection<Word> Words { get; set; }
    }
}
