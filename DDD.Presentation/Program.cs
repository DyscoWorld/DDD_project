using DDD.Domain.DomainEvents;
using DDD.Presentation.TelegramBotIntegration;
using Microsoft.Extensions.DependencyInjection;

namespace DDD.Presentation
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();

            services.AddTransient<DomainEventDispatcher>();

            services.AddTransient<Service>();

            var serviceProvider = services.BuildServiceProvider();

            BotInitializer.StartBot();

            var someService = serviceProvider.GetRequiredService<Service>();
            someService.DoSomething();
        }
    }
}