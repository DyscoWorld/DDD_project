using DDD.Infrastructure.Repositories.Interfaces;
using DDD.Models.Dtos;

using Telegram.Bot;

namespace DDD.Presentation.Handlers;

public class TrainingHandler(ITelegramBotClient botClient, IUserRepository userRepository)
{
    private List<UserOnTrainingDto> TraininingUsers { get; set; } = [];

    public async Task TrainUser(GetTrainingWordsResponseDto dto)
    {
        TraininingUsers.Add(new(dto.TelegramId, Models.Enums.TrainingStatusEnum.Training, ""));

        foreach (var word in dto.TrainingWords)
            await TrainOneWord(dto.TelegramId, word);

        TraininingUsers.Remove(GetUserOnTrainingByTelegramId(dto.TelegramId));
    }

    public async Task TrainOneWord(string telegramId, TrainingWordDto dto)
    {
        var trainingUser = GetUserOnTrainingByTelegramId(telegramId);

        var translation = dto.Translation;

        var requestToTrain = $"Введите на английском: {translation}";
        await botClient.SendMessage(
            chatId: telegramId,
            text: requestToTrain
        );

        while ( trainingUser.TrainingStatus is not Models.Enums.TrainingStatusEnum.EnteredWord )
            await Task.Delay(100);

        trainingUser = GetUserOnTrainingByTelegramId(telegramId);

        var textToSend = trainingUser.LastEnteredWord == dto.Name
            ? $"Вы правильно перевели слово"
            : $"Неправильно";

        await userRepository.IncreaseUserWordRank(telegramId, dto.Name);

        await botClient.SendMessage(
            chatId: telegramId,
            text: textToSend
        );
    }

    private UserOnTrainingDto GetUserOnTrainingByTelegramId(string telegramId)
    {
        return TraininingUsers.Find(x => x.TelegramId == telegramId)!;
    } 

    public void ContunueTraining(string telegramId, string enteredWord)
    {
        var user = GetUserOnTrainingByTelegramId(telegramId);
        var userIndex = TraininingUsers.IndexOf(user);

        var newUser = new UserOnTrainingDto(telegramId, Models.Enums.TrainingStatusEnum.EnteredWord, enteredWord);
        TraininingUsers[userIndex] = newUser;
    }
}
