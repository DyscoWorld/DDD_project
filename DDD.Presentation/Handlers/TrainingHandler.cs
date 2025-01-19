using DDD.Infrastructure.Repositories.Interfaces;
using DDD.Models.Dtos;
using DDD.Presentation.TelegramBotIntegration;

using DnsClient.Internal;

using Microsoft.Extensions.Logging;

using Telegram.Bot;

namespace DDD.Presentation.Handlers;

public class TrainingHandler(ILogger<TrainingHandler> logger, IUserRepository userRepository, GetTelegramBotClientService getTelegramBotClientService)
{
    private Dictionary<string, UserOnTrainingDto> TraininingUsersByTelegramIds { get; set; } = [];
    private ITelegramBotClient TelegramBotClient { get; set; } = null!;

    public async Task TrainUser(GetTrainingWordsResponseDto dto)
    {
        logger.LogInformation(message: "Начало обработки юзера");
        TelegramBotClient = getTelegramBotClientService.TelegramBotClient;
        TraininingUsersByTelegramIds.Add(dto.TelegramId, new(dto.TelegramId, Models.Enums.TrainingStatusEnum.Training, ""));

        foreach (var word in dto.TrainingWords)
            await TrainOneWord(dto.TelegramId, word);

        TraininingUsersByTelegramIds.Remove(dto.TelegramId);
    }

    public async Task TrainOneWord(string telegramId, TrainingWordDto dto)
    {
        var translation = dto.Translation;
        logger.LogInformation("Формируем первое слово запрос ы");

        var requestToTrain = $"Введите на английском: {translation}";
        await TelegramBotClient.SendMessage(
            chatId: telegramId,
            text: requestToTrain
        );

        logger.LogInformation("Отправили просьбу");

        while (TraininingUsersByTelegramIds[telegramId].TrainingStatus is not Models.Enums.TrainingStatusEnum.EnteredWord )
        {
            await Task.Delay(100);
        }

        string textToSend = "";

        if (TraininingUsersByTelegramIds[telegramId].LastEnteredWord == dto.Name)
        {
            textToSend = $"Вы правильно перевели слово";
            await userRepository.IncreaseUserWordRank(telegramId, dto.Name);
        }
        else
        {
            textToSend = $"Неправильно";
        }

        await TelegramBotClient.SendMessage(
            chatId: telegramId,
            text: textToSend
        );

        TraininingUsersByTelegramIds[telegramId] = new UserOnTrainingDto(telegramId, Models.Enums.TrainingStatusEnum.Training, "");
    }

    public void ContunueTraining(string telegramId, string enteredWord)
    {
        Console.WriteLine("Продолжаем тренировку 1");
        var newUser = new UserOnTrainingDto(telegramId, Models.Enums.TrainingStatusEnum.EnteredWord, enteredWord);
        TraininingUsersByTelegramIds[telegramId] = newUser;
        Console.WriteLine("Продолжаем тренировку 2");
    }
}
