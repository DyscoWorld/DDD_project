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

    public async Task<List<User>> GetScheduledTrainingsAsync()
    {
        var filter = Builders<User>.Filter.Where(u => u.Trainings != null && u.Trainings.Any());
        var users = await _users.Find(filter).ToListAsync();
        return users;
    }
}


