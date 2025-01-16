using DDD.Infrastructure.Data;
using MongoDB.Driver;

namespace DDD.Domain.DomainEvents
{
    public class LearnWord : IDomainEvent
    {
        public required int WordId { get; set; }

        private readonly IMongoCollection<WordEntity> _wordsCollection;

        public LearnWord(IMongoCollection<WordEntity> wordsCollection)
        {
            _wordsCollection = wordsCollection;
        }

        public void Handle()
        {
            var filter = Builders<WordEntity>.Filter.Eq(word => word.Id, WordId);
            var update = Builders<WordEntity>.Update.Set(word => word.IsLearned, true);

            _wordsCollection.UpdateOne(filter, update);
        }
    }
}
