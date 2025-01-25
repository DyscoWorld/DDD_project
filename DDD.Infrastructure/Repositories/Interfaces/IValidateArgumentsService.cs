using DDD.Models.Models;

namespace DDD.Infrastructure.Repositories.Interfaces;

/// <summary>
/// Валидация аргументов в командах
/// </summary>
public interface IValidateArgumentsService
{
    /// <summary>
    /// Валидация аргументов при добавлении новых слов
    /// </summary>
    public SplitAddWordArgumentsModel ValidateAndSplitAddWordArguments(string? arguments);
    
    /// <summary>
    /// Валидация настроек
    /// </summary>
    public SplitSettingsArgumentsModel ValidateAndSplitSettingsArguments(string? arguments);
}