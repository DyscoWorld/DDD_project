using DDD.Models.Data;
using DDD.Models.Repositories;
using DDD.Models.Services;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace DDD.Models
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IMongoDatabase>(sp =>
                {
                    var client = new MongoClient("mongodb://localhost:27017");
                    return client.GetDatabase("EnglishDB");
                })
                .AddSingleton<AppDbContext>()
                .AddTransient<WordRepository>()
                .AddTransient<UserRepository>()
                .AddTransient<UserSettingsRepository>()
                .AddTransient<WordService>()
                .AddTransient<UserSettingsService>()
                .BuildServiceProvider();

            var wordService = serviceProvider.GetService<WordService>();
            var userSettingsService = serviceProvider.GetService<UserSettingsService>();

            // Пример использования сервиса для добавления слова
            await wordService.AddWordAsync("123456789", "world", "мир", "Планета Земля", 1, 0, false);
            var words = await wordService.GetWordsAsync("123456789");
            foreach (var word in words)
            {
                Console.WriteLine($"{word.Name} - {word.Translation}");
            }

            // Пример использования сервиса для добавления настроек пользователя
            await userSettingsService.UpdateUserSettingsAsync("123456789", "30 minutes", 10);
            var userSettings = await userSettingsService.GetUserSettingsAsync("123456789");
            if (userSettings != null)
            {
                Console.WriteLine($"Time to spend: {userSettings.TimeToSpendMessages}, Word amount: {userSettings.WordAmount}");
            }
        }
    }
}
