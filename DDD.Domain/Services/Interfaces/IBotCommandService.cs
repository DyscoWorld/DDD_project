namespace DDD.Infrastructure.Repositories.Interfaces;

/// <summary>
/// Сервис комманд 
/// </summary>
public interface IBotCommandService
{
    /// <summary>
    /// Стартовая команда
    /// </summary>
    Task<string> HandleStartCommandAsync(string telegramId);
    
    /// <summary>
    /// Команда помощи
    /// </summary>
    Task<string> HandleHelpCommandAsync();
    
    /// <summary>
    /// Ошибка неизвестная команда
    /// </summary>
    Task<string> HandleUnknownCommandAsync();

    /// <summary>
    /// Добавление персонального слова
    /// </summary>
    Task<string> HandleAddWordCommandAsync(string telegramId, string commandText);
}