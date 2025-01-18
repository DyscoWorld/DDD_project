using DDD.Models.Enums;

namespace DDD.Models.Dtos;

public record UserOnTrainingDto(
    string TelegramId,
    TrainingStatusEnum TrainingStatus,
    string LastEnteredWord
);