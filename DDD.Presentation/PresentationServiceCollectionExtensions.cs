using DDD.Presentation.Handlers;
using DDD.Presentation.TelegramBotIntegration;
using DDD.Presentation.Workers;
using Microsoft.Extensions.DependencyInjection;

namespace DDD.Presentation;

public static class PresentationServiceCollectionExtensions
{
    public static void AddPresentationServices(this IServiceCollection services)
    {
        services.AddSingleton<GetTelegramBotClientService>();
        services.AddSingleton<BotInitializer>();
        
        services.AddScoped<CommandHandler>();
        services.AddScoped<UpdateHandler>();
        services.AddSingleton<TrainingHandler>();
        services.AddSingleton<Worker>();
        services.AddHostedService<Worker>();
    }
}