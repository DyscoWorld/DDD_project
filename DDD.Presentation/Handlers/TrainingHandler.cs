using DDD.Domain.DomainEvents;
using DDD.Infrastructure;
using DDD.Infrastructure.Repositories.Interfaces;
using DDD.Models.Dtos;
using DDD.Models.Enums;
using DDD.Models.Models;
using DDD.Presentation.TelegramBotIntegration;
using Telegram.Bot;

namespace DDD.Presentation.Handlers;

public class TrainingHandler(
    IUserRepository userRepository,
    GetTelegramBotClientService getTelegramBotClientService,
    GetTrainingWordsEvent trainingEvent)
{
    private Dictionary<string, UserOnTrainingModel> TraininingUsersByTelegramIds { get; set; } = [];
    private ITelegramBotClient TelegramBotClient { get; set; } = null!;

    public async Task TrainUser(string telegramId)
    {
        TelegramBotClient = getTelegramBotClientService.TelegramBotClient;
        
        if (!TraininingUsersByTelegramIds.ContainsKey(telegramId))
        {
            var dto = await trainingEvent.Handle(new(telegramId));
            TraininingUsersByTelegramIds.Add(
                dto.TelegramId,
                new UserOnTrainingModel(
                    dto.TelegramId, 
                    TrainingStatus.Training, 
                    "", 
                    dto.TrainingWords
                    ));
        }
        var user = TraininingUsersByTelegramIds[telegramId];

        await TelegramBotClient.SendMessage(
            chatId: user.TelegramId,
            text: "Начинаем тренировку"
        );

        user.CurrentWord = user.Words.Pop();
        await SendWord(user.TelegramId, user.CurrentWord);
    }

    public bool IsUserOnTraining(string telegramId)
        => TraininingUsersByTelegramIds.ContainsKey(telegramId)
           && TraininingUsersByTelegramIds[telegramId].TrainingStatus != TrainingStatus.NotTraining;
    
    public async Task UpdateTraining(string telegramId, string enteredWord, TrainingStatus status)
    {
        var user = TraininingUsersByTelegramIds[telegramId];
        var word = user.CurrentWord;
        user.TrainingStatus = status;
        user.LastEnteredWord = enteredWord;

        if (user.Words.Count >= 0 && status == TrainingStatus.EnteredWord)
        {
            string textToSend;
            if (enteredWord.Equals(word.Name, StringComparison.OrdinalIgnoreCase))
            {
                textToSend = $"Вы правильно перевели слово";
                await userRepository.IncreaseUserWordRank(telegramId, word.Name);
            }
            else
            {
                textToSend = $"Неправильно";
            }

            await SendMessage(telegramId, textToSend);

            if (user.Words.Count == 0)
            {
                TraininingUsersByTelegramIds.Remove(telegramId);
                
                await TelegramBotClient.SendMessage(
                    chatId: telegramId,
                    text: "Тренировка закончена"
                );
            }
            else
            {
                user.CurrentWord = user.Words.Pop();
                await SendWord(user.TelegramId, user.CurrentWord);
            }
        }
        
    }

    private async Task SendMessage(string telegramId, string textToSend)
    {
        await TelegramBotClient.SendMessage(
            chatId: telegramId,
            text: textToSend
        );
    }
    
    private async Task SendWord(string telegramId, TrainingWordDto dto)
    {
        var translation = dto.Translation;
        
        var requestToTrain = $"Введите на английском: {translation}";
        await TelegramBotClient.SendMessage(
            chatId: telegramId,
            text: requestToTrain
        );
    }

    private void UpdateUserOnTraining(UserOnTrainingModel user, TrainingStatus status, string enteredWord)
    {
        user.TrainingStatus = status;
        user.LastEnteredWord = enteredWord;
    }
}