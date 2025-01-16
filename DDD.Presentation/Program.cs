using DDD.Domain.DomainEvents;
using DDD.Infrastructure;
using DDD.Infrastructure.Data;
using DDD.Infrastructure.Repositories;
using DDD.Infrastructure.Repositories.Interfaces;
using DDD.Presentation.TelegramBotIntegration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using SchedulerService;

namespace DDD.Presentation
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddLogging(builder =>
                    {
                        builder.AddConsole();
                        builder.SetMinimumLevel(LogLevel.Information);
                    });

                    services.AddSingleton<IMongoClient>(sp => new MongoClient("mongodb://admin:password@localhost:27017"));
                    services.AddSingleton(sp => sp.GetRequiredService<IMongoClient>().GetDatabase("YourDatabaseName"));
                    services.AddSingleton(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<WordEntity>("Words"));
                    services.AddSingleton(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<SettingsEntity>("Settings"));

                    services.AddSingleton<ApplicationDbContextTemplate>();
                    services.AddTransient<IUserTrainingRepositoryTemplate, UserTrainingRepository>();

                    services.AddTransient<DomainEventDispatcher>();
                    services.AddTransient<Service>();

                    services.AddTransient<AddSingleWord>();
                    services.AddTransient<ListWords>();
                    services.AddTransient<LearnWord>();
                    services.AddTransient<SettingsEvent>();

                    services.AddHostedService<Worker>();
                })
                .Build();

            await BotInitializer.StartBotAsync();

            Console.WriteLine("Для остановки приложения нажмите Ctrl+C");

            using (host)
            {
                var lifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();

                Console.CancelKeyPress += (sender, eventArgs) =>
                {
                    eventArgs.Cancel = true;
                    lifetime.StopApplication();
                };

                await host.RunAsync();
            }

            BotInitializer.StopBot();
        }
    }
}
