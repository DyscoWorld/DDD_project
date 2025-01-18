using DDD.Infrastructure.Repositories.Interfaces;
using DDD.Models.Models;
using MongoDB.Driver;

namespace DDD.Infrastructure.Repositories;

/// <inheritdoc/>
public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<User> _users;

    public UserRepository(AppDbContext context)
    {
        _users = context.Users;
    }

    public async Task<User> GetByTelegramIdAsync(string telegramId)
    {
        return await _users.Find(u => u.TelegramId == telegramId).FirstOrDefaultAsync();
    }

    public async Task AddAsync(User user)
    {
        await _users.InsertOneAsync(user);
    }

    public async Task UpdateAsync(User user)
    {
        await _users.ReplaceOneAsync(u => u.Id == user.Id, user);
    }

    public async Task AddPersonalWord(string telegramId, Word word)
    {
        var user = await GetByTelegramIdAsync(telegramId);
        user.PersonalWords.Add(word);

        await UpdateAsync(user);
    }

    public async Task<List<Word>> GetAllPersonalWords(string telegramId)
    {
        var user = await GetByTelegramIdAsync(telegramId);

        return user.PersonalWords.ToList();
    }

    public async Task<Settings> GetSettings(string telegramId)
    {
        var user = await GetByTelegramIdAsync(telegramId);

        return user.Settings;
    }

    public async Task<List<User>> GetAllUsers()
    {
        return _users.AsQueryable().ToList();
    }

    public async Task IncreaseUserWordRank(string telegramId, string wordName)
    {
        var user = await GetByTelegramIdAsync(telegramId);

        var word = user.PersonalWords.First(x => x.Name == wordName);

        word.Rank += 1;

        if (word.Rank >= 10)
            word.IsLearned = true;

        await UpdateAsync(user);
    }
}


