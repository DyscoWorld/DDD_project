using DDD.Models.Dtos;
using DDD.Models.Enums;

namespace DDD.Models.Models;

public class UserOnTrainingModel
{
    public string TelegramId;
    public TrainingStatus TrainingStatus;
    public string LastEnteredWord;
    public TrainingWordDto CurrentWord;
    public List<TrainingWordDto> Words;

    public UserOnTrainingModel(string telegramId, TrainingStatus trainingStatus, string lastEnteredWord, List<TrainingWordDto> words)
    {
        TelegramId = telegramId;
        TrainingStatus = trainingStatus;
        LastEnteredWord = lastEnteredWord;
        Words = words;
    }
}