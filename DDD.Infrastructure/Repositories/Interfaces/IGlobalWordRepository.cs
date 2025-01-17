using DDD.Models.Models;

namespace DDD.Infrastructure.Repositories.Interfaces;

/// <summary>
/// 
/// </summary>
public interface IGlobalWordRepository
{
    /// <summary>
    /// 
    /// </summary>
    Task<List<GlobalWord>> GetAllAsync();
    
    /// <summary>
    /// 
    /// </summary>
    Task<GlobalWord> GetByIdAsync(string id);
    
    /// <summary>
    /// 
    /// </summary>
    Task AddAsync(GlobalWord word);
    
    /// <summary>
    /// 
    /// </summary>
    Task UpdateAsync(GlobalWord word);
}