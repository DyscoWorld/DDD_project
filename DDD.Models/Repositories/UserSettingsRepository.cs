using DDD.Models.Data;
using DDD.Models.Models;
using MongoDB.Driver;

namespace DDD.Models.Repositories
{
    public class UserSettingsRepository
    {
        private readonly AppDbContext _context;

        public UserSettingsRepository(AppDbContext context) => _context = context;

        public async Task<UserSettings> GetUserSettingsAsync(string userId)
        {
            var user = await _context.Users.Find(u => u.Id == userId).FirstOrDefaultAsync();
            return user?.Settings.FirstOrDefault();
        }

        public async Task AddUserSettingsAsync(string userId, UserSettings settings)
        {
            var user = await _context.Users.Find(u => u.Id == userId).FirstOrDefaultAsync();
            if (user != null)
            {
                user.Settings.Add(settings);
                await _context.Users.ReplaceOneAsync(u => u.Id == user.Id, user);
            }
        }

        public async Task UpdateUserSettingsAsync(string userId, UserSettings settings)
        {
            var user = await _context.Users.Find(u => u.Id == userId).FirstOrDefaultAsync();
            if (user != null)
            {
                var existingSettings = user.Settings.FirstOrDefault();
                if (existingSettings != null)
                {
                    existingSettings.TimeToSpendMessages = settings.TimeToSpendMessages;
                    existingSettings.WordAmount = settings.WordAmount;
                    await _context.Users.ReplaceOneAsync(u => u.Id == user.Id, user);
                }
            }
        }
    }

}
