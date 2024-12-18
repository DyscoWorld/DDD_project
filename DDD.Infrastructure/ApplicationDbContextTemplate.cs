using DDD.Models.Entities;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace DDD.Infrastructure;

/// <summary>
/// Временный dbcontext для проверки SchedulerService
/// </summary>
public class ApplicationDbContextTemplate
{
    private readonly IMongoDatabase _database;

    public ApplicationDbContextTemplate(IMongoClient mongoClient)
    {
        _database = mongoClient.GetDatabase("databaseMongo");
    }

    public IMongoCollection<UserTrainingTemplate> UserTrainings => _database.GetCollection<UserTrainingTemplate>("UserTrainings");
}
