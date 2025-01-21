using DDD.Infrastructure.Repositories.Interfaces;
using DDD.Models.Dtos;
using DDD.Models.Models;

namespace DDD.Domain.DomainEvents;

// Вывод списка слов пользователя
public class ListWords : IDomainEvent<ListWordsDto, List<Word>>
{
    private readonly IUserRepository userRepository;

    public ListWords(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task<List<Word>> Handle(ListWordsDto dto)
    {
        var words = await userRepository.GetAllPersonalWords(dto.TelegramId);
        return words ?? new List<Word>();
    }
}
