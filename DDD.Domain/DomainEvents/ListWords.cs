using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Infrastructure.Data;
using MongoDB.Driver;

namespace DDD.Domain.DomainEvents
{
    public class ListWords : IDomainEvent
    {
        public List<WordEntity> Result { get; private set; } = new List<WordEntity>();

        private readonly IMongoCollection<WordEntity> _wordsCollection;

        public ListWords(IMongoCollection<WordEntity> wordsCollection)
        {
            _wordsCollection = wordsCollection;
        }

        public void Handle()
        {
            Result = _wordsCollection.Find(_ => true).ToList();
        }
    }
}
