using DDD.Infrastructure.Repositories.Interfaces;
using DDD.Models.Dtos;

namespace DDD.Domain.DomainEvents;

public class CreateUserEvent(IUserRepository userRepository) : IDomainEvent<CreateUserDto, bool>
{
    public async Task<bool> Handle(CreateUserDto dto)
    {
        var userOrNull = await userRepository.GetByTelegramIdAsync(dto.ChatId);

        if (userOrNull is null)
            await userRepository.CreateUser(dto.ChatId);

        return userOrNull is null;
    }
}
