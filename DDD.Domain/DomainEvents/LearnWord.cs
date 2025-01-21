using DDD.Infrastructure.Repositories.Interfaces;
using DDD.Models.Dtos;
using DDD.Models.Models;

namespace DDD.Domain.DomainEvents;

// Изучение слова
public class LearnWord(IUserRepository userRepository) : IDomainEvent<LearnWordDto, bool>
{
    public async Task<bool> Handle(LearnWordDto dto)
    {
        var user = await userRepository.GetByTelegramIdAsync(dto.TelegramId);
        if (user == null) return false;

        var word = user.PersonalWords.FirstOrDefault(w => w.Name == dto.WordName);
        if (word == null) return false;

        if (word.Rank >= dto.MaxRank)
        {
            word.IsLearned = true;
            await userRepository.UpdateAsync(user);
            return true;
        }

        return false;
    }
}