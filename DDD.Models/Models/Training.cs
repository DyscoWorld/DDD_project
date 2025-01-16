using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DDD.Models.Models
{
    /// <summary>
    /// Тренировка
    /// </summary>
    public class Training
    {
        /// <summary>
        /// Время для оповещения
        /// </summary>
        public string TimeToSpendMessages { get; set; }
        
        /// <summary>
        /// Кол-во слов в тренировке
        /// </summary>
        public int WordAmount { get; set; }
    }
}
