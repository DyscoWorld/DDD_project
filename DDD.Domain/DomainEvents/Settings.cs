using DDD.Infrastructure.Repositories.Interfaces;
using DDD.Models.Dtos;
using DDD.Models.Models;

namespace DDD.Domain.DomainEvents;

// Настройка параметров пользователя
public class Settings(IUserRepository userRepository) : IDomainEvent<SettingsDto, bool>
{
    public async Task<bool> Handle(SettingsDto dto)
    {
        return await userRepository.UpdateUserSettingsAsync(dto.TelegramId, dto.TimeToSpendMessages, dto.WordAmount);
    }
}
