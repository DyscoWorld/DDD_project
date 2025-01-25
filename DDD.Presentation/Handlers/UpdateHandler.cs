using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using DDD.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using DDD.Models.Dtos;
using DDD.Models.Models;

namespace DDD.Presentation.Handlers;

/// <summary>
/// Класс handler для сообщений
/// </summary>
public class UpdateHandler(CommandHandler commandHandler, ILogger<UpdateHandler> logger)
{
    /// <summary>
    /// Обработка входящих сообщений
    /// </summary>
    public async Task HandleUpdateMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Type == UpdateType.Message && update.Message is not null)
        {
            var messageObject = update.Message;
            var chatId = messageObject.Chat.Id.ToString();
            var textOfMessage = messageObject.Text?.ToLower();

            if (textOfMessage is null)
                return;

            logger.LogInformation("Получено сообщение от {ChatId}: {Text}", chatId, textOfMessage);

            if (IsCommand(textOfMessage))
                await commandHandler.HandleAnyCommand(MapToCommandDto(textOfMessage, chatId), cancellationToken);
            else
                await commandHandler.HandleNotCommand(textOfMessage, chatId, cancellationToken);
        }
    }

    /// <summary>
    /// Обработка ошибок
    /// </summary>
    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Произошла ошибка");
        return Task.CompletedTask;
    }

    private static bool IsCommand(string? message) => message is not null && message.StartsWith('/') && message.Length > 1;

    private static CommandModel MapToCommandDto(string message, string chatId)
    {
        if (message.Split(' ').Length == 1)
            return new CommandModel(message, null, chatId);

        var command = message[..message.IndexOf(' ')];
        var arguments = message[(message.IndexOf(' ') + 1)..];

        return new CommandModel(command, arguments, chatId);
    }
}