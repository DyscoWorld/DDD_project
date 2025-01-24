using DDD.Infrastructure.Repositories.Interfaces;
using DDD.Models.Dtos;

namespace DDD.Domain.DomainEvents;

// Вывод списка слов пользователя
public record ListWordsEvent(IUserRepository UserRepository)
    : IDomainEvent<ListWordsRequestDto, ListWordsResponseDto>
{
    public async Task<ListWordsResponseDto> Handle(ListWordsRequestDto dto)
    {
        var words = await UserRepository.GetAllPersonalWords(dto.TelegramId);
        return new ListWordsResponseDto(words ?? []);
    }
}
