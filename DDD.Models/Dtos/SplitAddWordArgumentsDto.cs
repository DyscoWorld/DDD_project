namespace DDD.Models.Dtos;

public record SplitAddWordArgumentsDto(
    bool IsValid,
    string Word,
    string Translation,
    string Description
) : BaseDto;
