using DDD.Domain.DomainEvents;
using DDD.Models.Dtos;
using DDD.Presentation.TelegramBotIntegration;

using Telegram.Bot;

namespace DDD.Presentation.Handlers;

public class CommandHandler(
    GetTelegramBotClientService getTelegramBotClientService,
    TrainingHandler trainingHandler,
    CreateUserEvent createUserEvent,
    AddSingleWordEvent addSingleWordEvent
)
{
    private ITelegramBotClient TelegramBotClient = null!;

    public async Task HandleAnyCommand(CommandDto command, CancellationToken cancellationToken)
    {
        TelegramBotClient ??= getTelegramBotClientService.TelegramBotClient;

        switch (command.Command)
        {
            case "/addword":
                await HandleAddWordCommand(command.Arguments, command.ChatId);
                break;
            case "/start":
                await HandleStartCommand(command.ChatId);
                break;
            case "/help":
                await HandleHelpCommand(command.ChatId);
                break;
            default:
                await HandleUnkownCommand(command.ChatId);
                break;
        }
    }

    public async Task HandleNotCommand(string message, string chatId, CancellationToken cancellationToken)
    {
        TelegramBotClient ??= getTelegramBotClientService.TelegramBotClient;

        if (trainingHandler.IsUserOnTraining(telegramId: chatId))
            trainingHandler.ContunueTraining(chatId, message);
        else
            await SendMessageToUser(chatId, "Вы не на тренировке, введите команду");
    }

    public async Task HandleAddWordCommand(string? arguments, string chatId)
    {
        var splittedArguments = ValidateAndSplitAddWordArguments(arguments);
        if (!splittedArguments.IsValid)
        {
            await SendMessageToUser(chatId, "Невалидный синтаксис команды. Посмотрите в справке бота по команде /help");
            return;
        }

        await addSingleWordEvent.Handle(new AddSingleWordDto(chatId,
                                                             splittedArguments.Word,
                                                             splittedArguments.Translation,
                                                             splittedArguments.Description)
        );

        await SendMessageToUser(chatId, "Слово успешно добавлено");
    }

    public async Task HandleStartCommand(string chatId)
    {
        var isUserCreated = await createUserEvent.Handle(new(chatId));

        if (isUserCreated)
            await SendMessageToUser(chatId, "Вы успешно зарегистрировались в боте");
        else
            await SendMessageToUser(chatId, "Вы уже зарегистрированы");
    }

    public async Task HandleHelpCommand(string chatId)
    {
        var message = """
            Вот список доступных команд: 
            /start - Запуск бота
            /help - Помощь
            /addword 
            <английское слово или словосочетание>:<перевод>:<описание> - Добавить личное слово. Обязательно разделяйте двоеточием все аргументы в команде.
        """;

        await SendMessageToUser(chatId, message);
    }

    public async Task HandleUnkownCommand(string chatId)
    {
        var message = "Извините, я не понимаю эту команду.";
        await SendMessageToUser(chatId, message);
    }

    private async Task SendMessageToUser(string chatId, string message)
    {
        await TelegramBotClient.SendMessage(chatId: chatId, text: message, cancellationToken: default);
    }

    private static SplitAddWordArgumentsDto ValidateAndSplitAddWordArguments(string? arguments)
    {
        if (arguments is null)
            return new SplitAddWordArgumentsDto(false, null!, null!, null!);

        var splittedParts = arguments.Split(':');

        return splittedParts.Length != 3
            ? new SplitAddWordArgumentsDto(false, null!, null!, null!)
            : new SplitAddWordArgumentsDto(true, splittedParts[0], splittedParts[1], splittedParts[2]);
    }
}
