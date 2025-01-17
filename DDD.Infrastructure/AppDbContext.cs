using DDD.Models.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace DDD.Infrastructure;

public class AppDbContext
{
    public IMongoCollection<User> Users { get; }
    public IMongoCollection<GlobalWord> GlobalWords { get; }

    public AppDbContext(IMongoDatabase database)
    {
        Users = database.GetCollection<User>("Users");
        GlobalWords = database.GetCollection<GlobalWord>("GlobalWords");
    }
}

