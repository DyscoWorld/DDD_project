using DDD.Infrastructure.Repositories.Interfaces;
using DDD.Models.Models;

namespace DDD.Domain.Services;

/// <summary>
/// Сервис для взаимодействия с общим пулом
/// </summary>
public class GlobalWordService : IGlobalWordService
{
    private readonly IGlobalWordRepository _globalWordRepository;

    public GlobalWordService(IGlobalWordRepository globalWordRepository)
    {
        _globalWordRepository = globalWordRepository;
    }

    public async Task AddGlobalWordAsync(string name, string translation, string definition, int rank)
    {
        var word = new GlobalWord
        {
            Name = name,
            Translation = translation,
            Definition = definition,
            Rank = rank
        };
        await _globalWordRepository.AddAsync(word);
    }

    public async Task<List<GlobalWord>> GetAllGlobalWordsAsync()
    {
        return await _globalWordRepository.GetAllAsync();
    }
}
