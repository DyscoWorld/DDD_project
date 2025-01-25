using DDD.Domain.DomainEvents;
using DDD.Infrastructure.Helpers;
using DDD.Infrastructure.Repositories.Interfaces;
using DDD.Models.Dtos;
using DDD.Models.Enums;
using DDD.Models.Models;
using DDD.Presentation.TelegramBotIntegration;
using DDD.Presentation.Workers;
using Telegram.Bot;

namespace DDD.Presentation.Handlers;

public class CommandHandler(
    IValidateArgumentsService validateArgumentsService,
    GetTelegramBotClientService getTelegramBotClientService,
    TrainingHandler trainingHandler,
    CreateUserEvent createUserEvent,
    AddSingleWordEvent addSingleWordEvent,
    ChangeSettingsEvent changeSettingsEvent,
    ListWordsEvent listWordsEvent
)
{
    private ITelegramBotClient TelegramBotClient = null!;

    public async Task HandleAnyCommand(CommandModel command, CancellationToken cancellationToken)
    {
        TelegramBotClient ??= getTelegramBotClientService.TelegramBotClient;

        switch (command.Command)
        {
            case "/add_word":
                await HandleAddWordCommand(command.Arguments, command.ChatId);
                break;
            case "/start":
                await HandleStartCommand(command.ChatId);
                break;
            case "/help":
                await HandleHelpCommand(command.ChatId);
                break;
            case "/change_settings":
                await HandleSettingsCommand(command.Arguments, command.ChatId);
                break;
            case "/list_words":
                await HandleListWordsCommand(command.ChatId);
                break;
            case "/training":
                await HandleTrainingCommand(command.ChatId);
                break;
            default:
                await HandleUnkownCommand(command.ChatId);
                break;
        }
    }

    public async Task HandleTrainingCommand(string telegramId)
    {
        await trainingHandler.TrainUser(telegramId);
    }

    public async Task HandleNotCommand(string message, string chatId, CancellationToken cancellationToken)
    {
        TelegramBotClient ??= getTelegramBotClientService.TelegramBotClient;

        if (trainingHandler.IsUserOnTraining(chatId))
        {
            await trainingHandler.UpdateTraining(chatId, message, TrainingStatus.EnteredWord);
        }
        else
        {
            await TelegramBotClient.SendMessage(chatId, "Вы не на тренировке. Введите команду, чтобы начать тренировку.");
        }
    }

    public async Task HandleAddWordCommand(string? arguments, string chatId)
    {
        var splittedArguments = validateArgumentsService.ValidateAndSplitAddWordArguments(arguments);
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

    public async Task HandleSettingsCommand(string? arguments, string chatId)
    {
        var argumentsDto = validateArgumentsService.ValidateAndSplitSettingsArguments(arguments);
        if (!argumentsDto.IsValid)
        {
            await SendMessageToUser(chatId, "Невалидный синтаксис команды. Посмотрите в справке бота по команде /help");
            return;
        }

        await changeSettingsEvent.Handle(new(chatId, argumentsDto.TimeToSpendMessages!.Value, argumentsDto.WordAmount!.Value));
        await SendMessageToUser(chatId, "Настройки успешно изменены");
    }

    public async Task HandleListWordsCommand(string chatId)
    {
        var allWords = await listWordsEvent.Handle(new(chatId));
        if (allWords.Words.Count == 0)
            await SendMessageToUser(chatId, "У вас пока что нет добавленных слов. Посмотрите команду /help");

        var formattedWords = allWords.Words.Select(word => $"{word.Name} - {word.Translation} - {word.Definition}").ToList();
        var messageOfWords = "Ваши слова:\n" + string.Join('\n', formattedWords);

        await SendMessageToUser(chatId, messageOfWords);
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
            /start - Запуск бота.
            /help - Помощь.
            /add_word <английское слово или словосочетание>:<перевод>:<описание> - Добавить личное слово. Обязательно разделяйте двоеточием все аргументы в команде.
            /change_settings <время тренировки>:<количество слов за тренировку> - Поменять настройки. Обязательно разделяйте двоеточием все аргументы в команде.
            /list_words - Вывести все добавленные слова.
            /training - запустить тренировку сейчас.
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
}
