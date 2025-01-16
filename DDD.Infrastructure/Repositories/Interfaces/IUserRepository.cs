using DDD.Models.Models;

namespace DDD.Infrastructure.Repositories.Interfaces;

/// <summary>
/// Репозиторий бд User
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Получить пользователя по TelegramId.
    /// </summary>
    Task<User> GetByTelegramIdAsync(string telegramId);

    /// <summary>
    /// Добавить нового пользователя в базу данных.
    /// </summary>
    Task AddAsync(User user);

    /// <summary>
    /// Обновить данные пользователя в базе данных.
    /// </summary>
    Task UpdateAsync(User user);

    /// <summary>
    /// Возвращает список пользователей, у которых имеется хотя бы одна запланированная тренировка.
    /// </summary>
    Task<List<User>> GetScheduledTrainingsAsync();
}


