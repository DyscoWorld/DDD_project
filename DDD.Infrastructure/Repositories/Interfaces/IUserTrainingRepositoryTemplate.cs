using DDD.Models.Entities;

namespace DDD.Infrastructure.Repositories.Interfaces;


/// <summary>
/// Временный репозиторий бд для проверки SchedulerService
/// </summary>
public interface IUserTrainingRepositoryTemplate
{
    Task<List<UserTrainingTemplate>> GetScheduledTrainingsAsync();
}

