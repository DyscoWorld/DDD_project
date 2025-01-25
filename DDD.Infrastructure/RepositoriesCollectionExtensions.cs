using DDD.Infrastructure.Repositories;
using DDD.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DDD.Infrastructure;

public static class RepositoriesCollectionExtensions
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
    }
}