using DDD.Infrastructure.Repositories;
using DDD.Infrastructure.Repositories.Interfaces;
using DDD.Models.Dtos;
using DDD.Models.Models;

namespace DDD.Domain.DomainEvents;

// Добавление слова
public class AddSingleWord(IUserRepository userRepository) : IDomainEvent<AddSingleWordDto, int>
{
    public async Task<int> Handle(AddSingleWordDto dto)
    {
        var word = new Word()
        {
            Name = dto.Name,
            Definition = dto.Definition,
            Translation = dto.Translation,
            Rank = 0,
            IsLearned = false
        };

        await userRepository.AddPersonalWord(dto.TelegramId, word);

        return 0;
    }
}
