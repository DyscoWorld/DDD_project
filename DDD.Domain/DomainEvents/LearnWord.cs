using DDD.Infrastructure.Repositories.Interfaces;
using DDD.Models.Dtos;
using DDD.Models.Models;

namespace DDD.Domain.DomainEvents;

// Изучение слова
public class LearnWord(IUserRepository userRepository) : IDomainEvent<LearnWordDto, bool>
{
    public async Task<bool> Handle(LearnWordDto dto)
    {
        return await userRepository.LearnWordAsync(dto.TelegramId, dto.WordName, dto.MaxRank);
    }
}