namespace DDD.Models.Dtos;

public record SplitSettingsArgumentsDto(
    bool IsValid,
    TimeOnly? TimeToSpendMessages,
    int? WordAmount
) : BaseDto;
