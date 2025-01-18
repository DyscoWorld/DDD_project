using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace DDD.Models.Models
{
    /// <summary>
    /// Модель пользователя
    /// </summary>
    public class User
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        /// <summary>
        /// Идентификатор телеграмма пользователя 
        /// </summary>
        public string TelegramId { get; set; }
        
        /// <summary>
        /// Дата последней активности
        /// </summary>
        public DateTime LastActivity { get; set; }
        
        /// <summary>
        /// Запланированные тренировки
        /// </summary>
        public Settings Settings { get; set; } = new Settings();
        
        /// <summary>
        /// Слова пользователя
        /// </summary>
        public ICollection<Word> PersonalWords { get; set; } = new List<Word>();
    }
}
