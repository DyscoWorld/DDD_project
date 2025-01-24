using DDD.Models.Models;

namespace DDD.Models.Dtos;

public record ListWordsResponseDto(
    List<Word> Words
) : BaseDto;
