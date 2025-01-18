using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DDD.Models.Models
{
    /// <summary>
    /// Тренировка
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// Время для оповещения
        /// </summary>
        public TimeOnly TimeToSpendMessages { get; set; } = new TimeOnly(12, 0, 0);

        /// <summary>
        /// Кол-во слов в тренировке
        /// </summary>
        public int WordAmount { get; set; } = 5;
    }
}
