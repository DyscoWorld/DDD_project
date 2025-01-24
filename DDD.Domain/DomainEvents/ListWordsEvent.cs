using DDD.Infrastructure.Repositories.Interfaces;
using DDD.Models.Dtos;
using DDD.Models.Models;

namespace DDD.Domain.DomainEvents;

// Вывод списка слов пользователя
public record ListWordsEvent(IUserRepository UserRepository)
    : IDomainEvent<ListWordsDto, List<Word>>
{
    public async Task<List<Word>> Handle(ListWordsDto dto)
    {
        var words = await UserRepository.GetAllPersonalWords(dto.TelegramId);
        return words ?? new List<Word>();
    }
}
