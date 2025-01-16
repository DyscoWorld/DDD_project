using DDD.Infrastructure.Repositories.Interfaces;
using DDD.Models.Models;

namespace DDD.Domain.Services;

/// <summary>
/// Сервис для взаимодействия с пользовательскими словами 
/// </summary>
public class UserWordService
{
    private readonly IUserRepository _userRepository;

    public UserWordService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    /// <summary>
    /// Добавить слово, которое будет существовать только у конкретного пользователя.
    /// </summary>
    public async Task AddPersonalWordAsync(
        string telegramId,
        string name,
        string translation,
        string definition,
        int rank,
        bool isLearned)
    {
        var user = await _userRepository.GetByTelegramIdAsync(telegramId);
        if (user == null)
        {
            user = new User
            {
                TelegramId = telegramId,
                LastActivity = DateTime.UtcNow
            };
            await _userRepository.AddAsync(user);
        }
        
        var personalWord = new Word
        {
            Name = name,
            Translation = translation,
            Definition = definition,
            Rank = rank,
            IsLearned = isLearned
        };
        
        user.PersonalWords.Add(personalWord);
        await _userRepository.UpdateAsync(user);
    }

    /// <summary>
    /// Получить список личных слов пользователя
    /// </summary>
    public async Task<List<Word>> GetPersonalWordsAsync(string telegramId)
    {
        var user = await _userRepository.GetByTelegramIdAsync(telegramId);
        if (user == null) 
            throw new Exception("Пользователь не найден");

        return user.PersonalWords.ToList();
    }
}
