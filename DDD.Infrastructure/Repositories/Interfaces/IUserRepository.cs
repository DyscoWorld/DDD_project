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

    Task AddPersonalWord(string telegramId, Word word);
    
    Task<List<Word>> GetAllPersonalWords(string telegramId);

    Task<Settings> GetSettings(string telegramId);

    Task<List<User>> GetAllUsers();

    Task IncreaseUserWordRank(string telegramId, string wordName);
}


