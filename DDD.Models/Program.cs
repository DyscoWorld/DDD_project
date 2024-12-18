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

            // ПРИМЕР ДЛЯ ПРОВЕРКИ РАБОТОСПОСОБНОСТИ
            await wordService.AddWordAsync("123456789", "world", "мир");
            var words = await wordService.GetWordsAsync("123456789");
            foreach (var word in words)
            {
                Console.WriteLine($"{word.Name} - {word.Translation}");
            }
        }
    }
}
