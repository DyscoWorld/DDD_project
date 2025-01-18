namespace DDD.Models.Dtos;

public record GetTrainingWordsRequestDto(
    string TelegramId
) : BaseDto;