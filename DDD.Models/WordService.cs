using DDD.Models.Data;
using DDD.Models.Models;
using MongoDB.Driver;

namespace DDD.Models
{
    public class WordService
    {
        private readonly AppDbContext _context;

        public WordService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddWordAsync(string telegramId, string english, string translation)
        {
            var user = await _context.Users.Find(u => u.TelegramId == telegramId).FirstOrDefaultAsync();
            if (user == null)
            {
                user = new User { TelegramId = telegramId };
                await _context.Users.InsertOneAsync(user);
            }

            var word = new Word { Name = english, Translation = translation, UserId = user.Id };
            await _context.Words.InsertOneAsync(word);
        }

        public async Task<List<Word>> GetWordsAsync(string telegramId)
        {
            var user = await _context.Users.Find(u => u.TelegramId == telegramId).FirstOrDefaultAsync();
            if (user == null) return new List<Word>();

            return await _context.Words.Find(w => w.UserId == user.Id).ToListAsync();
        }
    }
}
