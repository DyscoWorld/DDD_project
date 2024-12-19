using DDD.Models.Data;
using DDD.Models.Models;
using MongoDB.Driver;

namespace DDD.Models.Repositories
{
    public class WordRepository
    {
        private readonly AppDbContext _context;

        public WordRepository(AppDbContext context) => _context = context;

        public async Task<Word> GetWordAsync(string id)
            => await _context.Words.Find(w => w.Id == id).FirstOrDefaultAsync();

        public async Task AddWordAsync(Word word)
            => await _context.Words.InsertOneAsync(word);

        public async Task UpdateWordAsync(Word word)
            => await _context.Words.ReplaceOneAsync(w => w.Id == word.Id, word);

        public async Task<List<Word>> GetWordsAsync(string userId)
            => await _context.Words.Find(w => w.UserId == userId).ToListAsync();

        public async Task<List<Word>> GetWordsByRankAsync(string userId)
            => await _context.Words.Find(w => w.UserId == userId).SortBy(w => w.Rank).ToListAsync();
    }

}
