namespace DDD.Models.Models;

// Command - вместе с "/"
public class CommandModel
{
    public string Command;
    public string? Arguments;
    public string ChatId;
    
    public CommandModel(string command, string? arguments, string chatId)
    {
        Command = command;
        Arguments = arguments;
        ChatId = chatId;
    }
};
