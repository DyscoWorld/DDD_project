namespace DDD.Models.Dtos;

// Command - вместе с "/"
public record CommandDto(string Command, string? Arguments, string ChatId);
