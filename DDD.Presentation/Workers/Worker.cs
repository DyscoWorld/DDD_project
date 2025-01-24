using DDD.Domain.DomainEvents;
using DDD.Infrastructure.Repositories.Interfaces;
using DDD.Models.Dtos;
using DDD.Models.Models;
using DDD.Presentation.Handlers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DDD.Presentation.Workers;

public class Worker(
    ILogger<Worker> logger,
    IUserRepository userRepository,
    TrainingHandler trainingHandler) : BackgroundService
{
    private AllUserSettingsModel AllUserSettings { get; set; }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("SchedulerService запущен.");

        while (!stoppingToken.IsCancellationRequested)
        {
            await UpdateUserSettingsAsync();
            var currentTime = DateTime.UtcNow;
            
            foreach (var settings in AllUserSettings.IdAndSettings)
            {
                if (!settings.TrainedToday)
                {
                    var timeToTrain = settings.Settings.TimeToSpendMessages;

                    if (timeToTrain.Hour == currentTime.Hour && timeToTrain.Minute == currentTime.Minute)
                    {
                        await trainingHandler.TrainUser(settings.TelegramId);
                        var userSettings = AllUserSettings.IdAndSettings.FirstOrDefault(x => x.TelegramId == settings.TelegramId);
                        userSettings.TrainedToday = true;
                        
                        // break нужен, чтобы так, как мы меняем массив, то чтобы не вылетало на foreach
                        break;
                    }
                }
            }
            
            await Task.Delay(10000, stoppingToken);
        }
    }
    
    private async Task UpdateUserSettingsAsync()
    {
        var users = await userRepository.GetAllUsers();

        if (AllUserSettings is null)
        {
            var mappedUsers = new List<TelegramIdAndSettingsModel>();
            foreach (var user in users)
            {
                mappedUsers.Add(await GetDtoForUser(user.TelegramId));
            }

            AllUserSettings = new AllUserSettingsModel
            {
                IdAndSettings = mappedUsers
            };
        }
        else
        {
            var mappedUsers = AllUserSettings.IdAndSettings;
            
            foreach (var user in users)
            {
                var mappedUser = mappedUsers.FirstOrDefault(x => x.TelegramId == user.TelegramId);
                if (mappedUser == null)
                {
                    mappedUsers.Add(await GetDtoForUser(user.TelegramId));
                }
                else
                {
                    mappedUser.Settings = await userRepository.GetSettings(user.TelegramId);
                }
            }
        }
        
    }
    
    private async Task<TelegramIdAndSettingsModel> GetDtoForUser(string telegramId) =>
        new()
        {
            TelegramId = telegramId,
            TrainedToday = false,
            Settings = await userRepository.GetSettings(telegramId)
        };
}
