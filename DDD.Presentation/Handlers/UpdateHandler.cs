using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using DDD.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace DDD.Presentation.Handlers;

/// <summary>
/// Класс handler для сообщений
/// </summary>
public class UpdateHandler
{
    private readonly IBotCommandService _botCommandService;
    private readonly ILogger<UpdateHandler> _logger;
    private readonly TrainingHandler _trainingHandler;

    public UpdateHandler(IBotCommandService botCommandService, ILogger<UpdateHandler> logger, TrainingHandler trainingHandler)
    {
        _botCommandService = botCommandService;
        _logger = logger;
        _trainingHandler = trainingHandler;
    }

    /// <summary>
    /// Обработка входящих сообщений
    /// </summary>
    public async Task HandleUpdateMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Type == UpdateType.Message && update.Message is not null)
        {
            var message = update.Message;
            var chatId = message.Chat.Id.ToString();
            var text = message.Text?.ToLower();

            _logger.LogInformation($"Получено сообщение от {chatId}: {text}");

            if (IsCommand(text))
            {
                string response;

                if (text != null && text.StartsWith("/addword"))
                {
                    response = await _botCommandService.HandleAddWordCommandAsync(chatId, text);
                }
                else
                {
                    response = text switch
                    {
                        "/start" => await _botCommandService.HandleStartCommandAsync(chatId),
                        "/help" => await _botCommandService.HandleHelpCommandAsync(),
                        _ => await _botCommandService.HandleUnknownCommandAsync()
                    };
                }

                await botClient.SendMessage(
                    chatId: message.Chat.Id,
                    text: response,
                    cancellationToken: cancellationToken
                );
            }
            else
            {
                TrainingHandler.ContunueTraining(chatId, text);
            }
        }
    }

    private bool IsCommand(string message) => message is not null && message.StartsWith('/');

    /// <summary>
    /// Обработка ошибок
    /// </summary>
    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Произошла ошибка");
        return Task.CompletedTask;
    }
}