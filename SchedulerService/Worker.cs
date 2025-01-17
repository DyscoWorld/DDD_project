using DDD.Infrastructure.Repositories.Interfaces;
using DDD.Models.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace SchedulerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IUserRepository _userRepository;

        public Worker(ILogger<Worker> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("SchedulerService запущен.");

            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.Now;

                // Получаем список пользователей с запланированными тренировками.
                // Метод должен быть реализован так, чтобы возвращать пользователей, у которых есть тренировки.
                var usersWithTrainings = await _userRepository.GetScheduledTrainingsAsync();

                foreach (var user in usersWithTrainings)
                {
                    foreach (var training in user.Trainings)
                    {
                        if (DateTime.TryParse(training.TimeToSpendMessages, out DateTime scheduledTime))
                        {
                            if (now >= scheduledTime)
                            {
                                _logger.LogInformation($"Выполняем обучение для пользователя {user.TelegramId} в {now}");


                                DoScheduledAction(user.TelegramId);
                            }
                        }
                        else
                        {
                            _logger.LogWarning($"Не удалось распарсить время для тренировки пользователя {user.TelegramId}: {training.TimeToSpendMessages}");
                        }
                    }
                }
                
                await Task.Delay(1000, stoppingToken);
            }
        }

        private void DoScheduledAction(string telegramId)
        {
            _logger.LogInformation($"Запланированное действие выполнено для пользователя {telegramId}!");
        }
    }
}
