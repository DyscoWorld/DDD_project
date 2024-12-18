using DDD.Infrastructure.Repositories.Interfaces;
using DDD.Models.Entities;
using MongoDB.Driver;

namespace DDD.Infrastructure.Repositories;

/// <inheritdoc/>
public class UserTrainingRepository : IUserTrainingRepositoryTemplate
{
    private readonly ApplicationDbContextTemplate _dbContext;

    public UserTrainingRepository(ApplicationDbContextTemplate dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<UserTrainingTemplate>> GetScheduledTrainingsAsync()
    {
        return await _dbContext.UserTrainings.Find(_ => true).ToListAsync();
    }
}