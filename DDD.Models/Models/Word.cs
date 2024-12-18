using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace DDD.Models.Models
{
    public class Word
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Translation { get; set; }
        public string Definition { get; set; }
        public int Rank { get; set; }
        public int TimesRepeated { get; set; }
        public bool IsLearned { get; set; }
        public string UserId { get; set; }
    }
}
