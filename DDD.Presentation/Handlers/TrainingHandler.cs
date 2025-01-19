using DDD.Infrastructure.Repositories.Interfaces;
using DDD.Models.Dtos;
using DDD.Presentation.TelegramBotIntegration;

using Telegram.Bot;

namespace DDD.Presentation.Handlers;

public class TrainingHandler(IUserRepository userRepository)
{
    private static Dictionary<string, UserOnTrainingDto> TraininingUsersByTelegramIds { get; set; } = [];
    private static ITelegramBotClient TelegramBotClient { get; set; } = TelegramBotClientSingleton.TelegramBotClient!;

    public async Task TrainUser(GetTrainingWordsResponseDto dto)
    {
        TraininingUsersByTelegramIds.Add(dto.TelegramId, new(dto.TelegramId, Models.Enums.TrainingStatusEnum.Training, ""));

        foreach (var word in dto.TrainingWords)
            await TrainOneWord(dto.TelegramId, word);

        TraininingUsersByTelegramIds.Remove(dto.TelegramId);
    }

    public async Task TrainOneWord(string telegramId, TrainingWordDto dto)
    {
        var translation = dto.Translation;

        var requestToTrain = $"Введите на английском: {translation}";
        await TelegramBotClient.SendMessage(
            chatId: telegramId,
            text: requestToTrain
        );

        while (TraininingUsersByTelegramIds[telegramId].TrainingStatus is not Models.Enums.TrainingStatusEnum.EnteredWord )
            await Task.Delay(100);

        var textToSend = TraininingUsersByTelegramIds[telegramId].LastEnteredWord == dto.Name
            ? $"Вы правильно перевели слово"
            : $"Неправильно";

        await userRepository.IncreaseUserWordRank(telegramId, dto.Name);

        await TelegramBotClient.SendMessage(
            chatId: telegramId,
            text: textToSend
        );
    }

    public static void ContunueTraining(string telegramId, string enteredWord)
    {
        var newUser = new UserOnTrainingDto(telegramId, Models.Enums.TrainingStatusEnum.EnteredWord, enteredWord);
        TraininingUsersByTelegramIds[telegramId] = newUser;
    }
}
