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
        public DateTime LastActivity { get; set; }
        public ICollection<UserSettings> Settings { get; set; } = new List<UserSettings>();
        public ICollection<string> WordIds { get; set; } = new List<string>();
    }
}
