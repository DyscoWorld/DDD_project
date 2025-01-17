using DDD.Infrastructure;
using DDD.Infrastructure.Repositories;
using DDD.Infrastructure.Repositories.Interfaces;
using MongoDB.Driver;
using SchedulerService;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddLogging(builder =>
        {
            builder.AddConsole();
            builder.SetMinimumLevel(LogLevel.Information);
        });
        
        services.AddSingleton<IMongoClient>(sp => new MongoClient("mongodb://admin:password@localhost:27017"));
        services.AddSingleton<AppDbContext>();
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();