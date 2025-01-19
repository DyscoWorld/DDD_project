using DDD.Domain.DomainEvents;
using DDD.Domain.Services;
using DDD.Infrastructure;
using DDD.Infrastructure.Repositories;
using DDD.Infrastructure.Repositories.Interfaces;
using DDD.Presentation.Handlers;
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
                    // Логирование
                    services.AddLogging(builder =>
                    {
                        builder.AddConsole();
                        builder.SetMinimumLevel(LogLevel.Information);
                    });

                    // Бд
                    services.AddSingleton<IMongoClient>(_ =>
                        new MongoClient("mongodb://admin:password@localhost:27017"));
                    services.AddSingleton<IMongoDatabase>(sp =>
                    {
                        var client = sp.GetRequiredService<IMongoClient>();
                        return client.GetDatabase("EnglishDB");
                    });
                    services.AddSingleton<AppDbContext>();

                    // Репозитории
                    services.AddScoped<IUserRepository, UserRepository>();

                    // Сервисы
                    services.AddScoped<UserWordService>();
                    services.AddScoped<IBotCommandService, BotCommandService>();

                    // Telegram Bot
                    services.AddScoped<UpdateHandler>();
                    services.AddSingleton<TrainingHandler>();
                    services.AddSingleton<BotInitializer>();
                    
                    services.AddHostedService<Worker>();

                    services.AddTransient<AddSingleWord>();
                    services.AddTransient<TrainingEvent>();
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

