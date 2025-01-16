using DDD.Infrastructure.Data;
using MongoDB.Driver;

namespace DDD.Domain.DomainEvents
{
    public class AddSingleWord : IDomainEvent
    {
        public required string Word { get; set; }

        private readonly IMongoCollection<WordEntity> _wordsCollection;

        public AddSingleWord(IMongoCollection<WordEntity> wordsCollection)
        {
            _wordsCollection = wordsCollection;
        }

        public void Handle()
        {
            var wordEntity = new WordEntity { Text = this.Word };
            _wordsCollection.InsertOne(wordEntity);
        }
    }
}
