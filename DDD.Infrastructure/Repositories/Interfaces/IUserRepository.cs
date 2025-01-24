using DDD.Models.Models;

namespace DDD.Infrastructure.Repositories.Interfaces;

/// <summary>
/// Репозиторий бд User
/// </summary>
public interface IUserRepository
{
    Task<User> GetByTelegramIdAsync(string telegramId);

    Task CreateUser(string telegramId);

    Task AddAsync(User user);

    Task UpdateAsync(User user);

    Task AddPersonalWord(string telegramId, Word word);
    
    Task<List<Word>> GetAllPersonalWords(string telegramId);

    Task<Settings> GetSettings(string telegramId);

    Task<List<User>> GetAllUsers();

    Task IncreaseUserWordRank(string telegramId, string wordName);

    Task<bool> LearnWordAsync(string telegramId, string wordName, int maxRank);

    Task<bool> UpdateUserSettingsAsync(string telegramId, TimeOnly timeToSpendMessages, int wordAmount);

}


