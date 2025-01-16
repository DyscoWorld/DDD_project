using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace DDD.Infrastructure.Data
{
    public class DataContext
    {
        private readonly IMongoDatabase _database;

        public DataContext()
        {
            var client = new MongoClient("YourMongoDBConnectionStringHere");
            _database = client.GetDatabase("YourDatabaseName");
        }

        public IMongoCollection<WordEntity> Words => _database.GetCollection<WordEntity>("Words");
        public IMongoCollection<SettingsEntity> Settings => _database.GetCollection<SettingsEntity>("Settings");
    }
}
