using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace DDD.Infrastructure;

public static class DatabaseServiceCollectionExtensions
{
    public static IServiceCollection AddMongoDatabase(this IServiceCollection services, string connectionString, string databaseName)
    {
        services.AddSingleton<IMongoClient>(_ => new MongoClient(connectionString));
        services.AddSingleton<IMongoDatabase>(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase(databaseName);
        });
        services.AddSingleton<AppDbContext>();

        return services;
    }
}