using DDD.Domain.DomainEvents;
using DDD.Infrastructure.Repositories;
using DDD.Infrastructure.Repositories.Interfaces;
using DDD.Models.Dtos;
using DDD.Presentation.Handlers;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Telegram.Bot;

namespace SchedulerService;

public class Worker(ILogger<Worker> logger, IUserRepository userRepository, TrainingEvent trainingEvent, TrainingHandler trainingHandler) : BackgroundService
{
    private AllUserSettingsDto AllUserSettings { get; set; }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var users = await userRepository.GetAllUsers();

        var mappedUsers = new List<TelegramIdAndSettingsDto>();
        foreach (var user in users)
            mappedUsers.Add(await GetDtoForUser(user.TelegramId));

        AllUserSettings = new AllUserSettingsDto(mappedUsers);

        logger.LogInformation("SchedulerService запущен.");

        while (!stoppingToken.IsCancellationRequested)
        {
            var currentTime = DateTime.Now;

            foreach (var settings in AllUserSettings.IdAndSettingsDtos)
            {
                if (!settings.TrainedToday)
                {
                    var timeToTrain = settings.Settings.TimeToSpendMessages;

                    if (timeToTrain.Hour == currentTime.Hour && timeToTrain.Minute == currentTime.Minute)
                    {
                        await TrainUser(settings.TelegramId);
                        var index = AllUserSettings.IdAndSettingsDtos.IndexOf(settings);
                        AllUserSettings.IdAndSettingsDtos[index] = new TelegramIdAndSettingsDto(settings.TelegramId, true, settings.Settings);
                        break;
                    }
                }
            }
            
            await Task.Delay(100, stoppingToken);
        }
    }

    private async Task TrainUser(string telegramId)
    {
        var trainingWords = await trainingEvent.Handle(new(telegramId));
        await trainingHandler.TrainUser(trainingWords);
    }

    private async Task<TelegramIdAndSettingsDto> GetDtoForUser(string telegramId) => 
        new TelegramIdAndSettingsDto(
            telegramId,
            false,
            await userRepository.GetSettings(telegramId)
        );
}
