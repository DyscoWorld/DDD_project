using DDD.Infrastructure.Repositories.Interfaces;
using DDD.Models.Models;
using MongoDB.Driver;

namespace DDD.Infrastructure.Repositories;

/// <inheritdoc/>
public class GlobalWordRepository : IGlobalWordRepository
{
    private readonly IMongoCollection<GlobalWord> _globalWords;

    public GlobalWordRepository(AppDbContext context)
    {
        _globalWords = context.GlobalWords; // Коллекция GlobalWords
    }

    public async Task<List<GlobalWord>> GetAllAsync()
    {
        return await _globalWords.Find(_ => true).ToListAsync();
    }

    public async Task<GlobalWord> GetByIdAsync(string id)
    {
        return await _globalWords.Find(g => g.Id == id).FirstOrDefaultAsync();
    }

    public async Task AddAsync(GlobalWord word)
    {
        await _globalWords.InsertOneAsync(word);
    }

    public async Task UpdateAsync(GlobalWord word)
    {
        await _globalWords.ReplaceOneAsync(g => g.Id == word.Id, word);
    }
}
