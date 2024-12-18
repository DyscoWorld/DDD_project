using DDD.Models.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Models.Data
{
    public class AppDbContext : DbContext
    {
        private readonly IMongoDatabase _database;

        public AppDbContext(IMongoDatabase database)
        {
            _database = database;
        }

        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
        public IMongoCollection<Word> Words => _database.GetCollection<Word>("Words");
        public IMongoCollection<UserSettings> Settings 
            => _database.GetCollection<UserSettings>("UserSettings");
    }
}
