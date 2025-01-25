using DDD.Domain;
using DDD.Domain.DomainEvents;
using DDD.Domain.Services;
using DDD.Infrastructure;
using DDD.Infrastructure.Repositories;
using DDD.Infrastructure.Repositories.Interfaces;
using DDD.Presentation.Handlers;
using DDD.Presentation.TelegramBotIntegration;
using DDD.Presentation.Workers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using MongoDB.Driver;

namespace DDD.Presentation
{
    class Program
    {
        private const string DbConnectionString = "mongodb://admin:password@localhost:27017";
        private const string DbName = "EnglishDB";
        
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
                    
                    services.AddMongoDatabase(DbConnectionString, DbName);
                    
                    services.AddRepositories();
                    
                    services.AddPresentationServices();
                    
                    services.AddDomainServices();
                })
                .Build();
            
            var botInitializer = host.Services.GetRequiredService<BotInitializer>();
            await botInitializer.StartBotAsync();

            Console.WriteLine("Для остановки приложения нажмите Ctrl+C");

            // Обработка Ctrl+C для корректного завершения приложения
            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                eventArgs.Cancel = true;
                botInitializer.StopBot();
                host.Services.GetRequiredService<IHostApplicationLifetime>().StopApplication();
            };

            // Запуск хоста
            await host.RunAsync();
        }
    }
}

