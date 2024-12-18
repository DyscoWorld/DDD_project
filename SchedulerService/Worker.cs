using DDD.Infrastructure.Repositories.Interfaces;

namespace SchedulerService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IUserTrainingRepositoryTemplate _userTrainingRepository;

    public Worker(ILogger<Worker> logger, IUserTrainingRepositoryTemplate userTrainingRepository)
    {
        _logger = logger;
        _userTrainingRepository = userTrainingRepository;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("SchedulerService запущен.");

        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.Now;

            // Получаем список запланированных обучений
            var scheduledTrainings = await _userTrainingRepository.GetScheduledTrainingsAsync();

            foreach (var training in scheduledTrainings)
            {
                if (now >= training.ScheduledTrainingTime)
                {
                    _logger.LogInformation($"Выполняем обучение для пользователя {training.UserId} в {DateTime.Now}");
                    
                    // Вызов события
                    // DomainEvent.SendWords(training.UserId);
                    DoScheduledAction();
                }
            }

            await Task.Delay(1000, stoppingToken);
        }
    }

    private void DoScheduledAction()
    {
        _logger.LogInformation("Запланированное действие выполнено!");
    }
}
