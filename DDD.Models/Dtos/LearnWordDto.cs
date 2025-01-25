
namespace DDD.Models.Dtos;

public record LearnWordDto(string TelegramId, string WordName, int MaxRank) : BaseDto;