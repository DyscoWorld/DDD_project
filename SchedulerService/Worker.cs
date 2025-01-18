using DDD.Domain.DomainEvents;
using DDD.Infrastructure.Repositories;
using DDD.Models.Dtos;

namespace SchedulerService;

public class Worker(ILogger<Worker> logger, UserRepository userRepository, TrainingEvent trainingEvent) : BackgroundService
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
            var currentTime = DateTime.UtcNow;

            foreach (var settings in AllUserSettings.IdAndSettingsDtos)
            {
                var timeToTrain = settings.Settings.TimeToSpendMessages;

                if (timeToTrain.Hour == currentTime.Hour && timeToTrain.Minute == currentTime.Minute)
                    await TrainUser(settings.TelegramId);
            }
            
            await Task.Delay(60001, stoppingToken);
        }
    }

    private async Task TrainUser(string telegramId)
    {
        var trainingWords =await trainingEvent.Handle(new(telegramId));
        

    }

    private async Task<TelegramIdAndSettingsDto> GetDtoForUser(string telegramId) => 
        new TelegramIdAndSettingsDto(
            telegramId,
            await userRepository.GetSettings(telegramId)
        );
}
