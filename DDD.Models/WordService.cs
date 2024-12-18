using DDD.Models.Data;
using DDD.Models.Models;
using MongoDB.Driver;

namespace DDD.Models
{
    public class WordService
    {
        private readonly AppDbContext _context;

        public WordService(AppDbContext context)
            => _context = context;

        public async Task AddWordAsync(string telegramId, string name, string translation, string definition, int rank, int timesRepeated, bool isLearned)
        {
            var user = await _context.Users.Find(u => u.TelegramId == telegramId).FirstOrDefaultAsync();
            if (user == null)
            {
                user = new User { TelegramId = telegramId, LastActivity = DateTime.UtcNow };
                await _context.Users.InsertOneAsync(user);
            }

            var word = new Word
            {
                Name = name,
                Translation = translation,
                Definition = definition,
                Rank = rank,
                TimesRepeated = timesRepeated,
                IsLearned = isLearned,
                UserId = user.Id
            };

            await _context.Words.InsertOneAsync(word);
            user.WordIds.Add(word.Id);
            await _context.Users.ReplaceOneAsync(u => u.Id == user.Id, user);
        }

        public async Task<List<Word>> GetWordsAsync(string telegramId)
        {
            var user = await _context.Users.Find(u => u.TelegramId == telegramId).FirstOrDefaultAsync();
            if (user == null) return new List<Word>();
            var words = await _context.Words.Find(w => user.WordIds.Contains(w.Id)).ToListAsync();
            return words;
        }

        public async Task AddUserSettingsAsync(string telegramId, string timeToSpendMessages, int wordAmount)
        {
            var userSettings = new UserSettings
            {
                TimeToSpendMessages = timeToSpendMessages,
                WordAmount = wordAmount
            };

            var user = await _context.Users.Find(u => u.TelegramId == telegramId).FirstOrDefaultAsync();
            if (user == null)
            {
                user = new User { TelegramId = telegramId, LastActivity = DateTime.UtcNow };
                await _context.Users.InsertOneAsync(user);
            }

            user.Settings.Add(userSettings);
            await _context.Users.ReplaceOneAsync(u => u.Id == user.Id, user);
        }

        public async Task<List<UserSettings>> GetUserSettingsAsync(string telegramId)
        {
            var user = await _context.Users.Find(u => u.TelegramId == telegramId).FirstOrDefaultAsync();
            if (user == null) return new List<UserSettings>();
            return user.Settings.ToList();
        }
    }
}
