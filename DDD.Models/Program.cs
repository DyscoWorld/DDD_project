using DDD.Models.Data;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

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
                .AddTransient<WordService>()
                .BuildServiceProvider();

            var wordService = serviceProvider.GetService<WordService>();

            // Пример использования сервиса для добавления слова
            await wordService.AddWordAsync("123456789", "world", "мир", "Планета Земля", 1, 0, false);
            var words = await wordService.GetWordsAsync("123456789");
            foreach (var word in words)
                Console.WriteLine($"{word.Name} - {word.Translation}");

            // Пример использования сервиса для добавления настроек пользователя
            await wordService.AddUserSettingsAsync("123456789", "30 minutes", 10);
            var userSettings = await wordService.GetUserSettingsAsync("123456789");
            foreach (var setting in userSettings)
                Console.WriteLine($"Time to spend: {setting.TimeToSpendMessages}, Word amount: {setting.WordAmount}");
        }
    }
}
