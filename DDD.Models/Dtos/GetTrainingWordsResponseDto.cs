namespace DDD.Models.Dtos;

public record GetTrainingWordsResponseDto(
    string TelegramId,
    List<TrainingWordDto> TrainingWords
) : BaseDto;

public record TrainingWordDto(
    string Name,
    string Translation,
    string Definition
) : BaseDto;