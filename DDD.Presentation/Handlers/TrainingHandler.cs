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
        TelegramBotClient = getTelegramBotClientService.TelegramBotClient;

        await TelegramBotClient.SendMessage(
            chatId: dto.TelegramId,
            text: "Начинаем тренировку"
        );

        if (!TraininingUsersByTelegramIds.ContainsKey(dto.TelegramId))
            TraininingUsersByTelegramIds.Add(dto.TelegramId, new(dto.TelegramId, Models.Enums.TrainingStatusEnum.NotTraining, ""));

        TraininingUsersByTelegramIds[dto.TelegramId] = new(dto.TelegramId, Models.Enums.TrainingStatusEnum.Training, "");

        foreach (var word in dto.TrainingWords)
            await TrainOneWord(dto.TelegramId, word);

        TraininingUsersByTelegramIds[dto.TelegramId] = new(dto.TelegramId, Models.Enums.TrainingStatusEnum.NotTraining, "");

        await TelegramBotClient.SendMessage(
            chatId: dto.TelegramId,
            text: "Тренировка закончена"
        );
    }

    public bool IsUserOnTraining(string telegramId) 
        => TraininingUsersByTelegramIds.ContainsKey(telegramId) 
        && TraininingUsersByTelegramIds[telegramId].TrainingStatus != Models.Enums.TrainingStatusEnum.NotTraining;

    public async Task TrainOneWord(string telegramId, TrainingWordDto dto)
    {
        var translation = dto.Translation;

        var requestToTrain = $"Введите на английском: {translation}";
        await TelegramBotClient.SendMessage(
            chatId: telegramId,
            text: requestToTrain
        );

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
        var newUser = new UserOnTrainingDto(telegramId, Models.Enums.TrainingStatusEnum.EnteredWord, enteredWord);
        TraininingUsersByTelegramIds[telegramId] = newUser;
    }
}
