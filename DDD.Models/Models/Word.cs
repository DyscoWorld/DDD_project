using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace DDD.Models.Models
{
    /// <summary>
    /// Личное слово пользователя
    /// </summary>
    public class Word
    {
        public string Name { get; set; }
        public string Translation { get; set; }
        public string Definition { get; set; }
        public int Rank { get; set; }
        public bool IsLearned { get; set; }
    }
}
