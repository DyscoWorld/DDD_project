using DDD.Infrastructure.Repositories.Interfaces;
using DDD.Models.Dtos;
using DDD.Models.Models;

namespace DDD.Domain.DomainEvents;

// Настройка параметров пользователя
public class Settings(IUserRepository userRepository) : IDomainEvent<SettingsDto, bool>
{
    public async Task<bool> Handle(SettingsDto dto)
    {
        var user = await userRepository.GetByTelegramIdAsync(dto.TelegramId);
        if (user == null) return false;

        user.Settings.TimeToSpendMessages = dto.TimeToSpendMessages;
        user.Settings.WordAmount = dto.WordAmount;

        await userRepository.UpdateAsync(user);
        return true;
    }
}
