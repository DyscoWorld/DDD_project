namespace DDD.Models.Dtos;

public record SettingsDto(string TelegramId, TimeOnly TimeToSpendMessages, int WordAmount) : BaseDto;