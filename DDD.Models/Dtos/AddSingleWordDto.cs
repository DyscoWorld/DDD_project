namespace DDD.Models.Dtos;

public record AddSingleWordDto(
    string TelegramId,
    string Name,
    string Translation,
    string Definition
) : BaseDto;
