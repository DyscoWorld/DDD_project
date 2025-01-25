using DDD.Domain.DomainEvents;
using DDD.Domain.Services;
using DDD.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DDD.Domain;

public static class DomainServiceCollectionExtensions
{
    public static void AddDomainServices(this IServiceCollection services)
    {
        services.AddTransient<IValidateArgumentsService, ValidateArgumentsService>();
        
        services.AddTransient<AddSingleWordEvent>();
        services.AddTransient<GetTrainingWordsEvent>();
        services.AddTransient<LearnWordEvent>();
        services.AddTransient<ListWordsEvent>();
        services.AddTransient<ChangeSettingsEvent>();
        services.AddTransient<CreateUserEvent>();
    }
}
