using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using DDD.Infrastructure.Repositories.Interfaces;
using DDD.Models.Models;
using DnsClient.Internal;
using Microsoft.Extensions.Logging;
using ILogger = DnsClient.Internal.ILogger;
using User = DDD.Models.Models.User;

namespace DDD.Presentation.Handlers
{
    /// <summary>
    /// Класс handler для сообщений
    /// </summary>
    public class UpdateHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UpdateHandler> _logger;

        public UpdateHandler(IUserRepository userRepository, ILogger<UpdateHandler> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        /// <summary>
        /// Обработка входящих сообщений
        /// </summary>
        public async Task HandleUpdateMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.Message && update.Message is not null)
            {
                var message = update.Message;

                Console.WriteLine($"Получено сообщение от {message.Chat.Id}: {message.Text}");

                if (message.Text is not null)
                {
                    string response;

                    switch (message.Text.ToLower())
                    {
                        case "/start":
                            response = await HandleStartCommandAsync(message.Chat.Id.ToString());
                            break;

                        case "/help":
                            response = "Вот список доступных команд:\n/start - запуск\n/help - помощь";
                            break;

                        default:
                            response = "Извините, я не понимаю эту команду.";
                            break;
                    }

                    await botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: response,
                        cancellationToken: cancellationToken
                    );
                }
            }
        }

        /// <summary>
        /// Обработка команды /start
        /// </summary>
        private async Task<string> HandleStartCommandAsync(string telegramId)
        {
            await CreateUserIfNotExistsAsync(telegramId);
            return "Привет! Я ваш бот. Чем могу помочь?";
        }

        /// <summary>
        /// Создание пользователя, если его нет в базе
        /// </summary>
        private async Task CreateUserIfNotExistsAsync(string telegramId)
        {
            // Проверяем, существует ли пользователь
            var user = await _userRepository.GetByTelegramIdAsync(telegramId);
            if (user == null)
            {
                // Создаём нового пользователя
                user = new User
                {
                    TelegramId = telegramId,
                    LastActivity = DateTime.UtcNow
                };

                await _userRepository.AddAsync(user);
                _logger.LogInformation($"Создан новый пользователь с TelegramId: {telegramId}");
            }
            else
            {
                _logger.LogInformation($"Пользователь с TelegramId: {telegramId} уже существует.");
            }
        }

        /// <summary>
        /// Обработка ошибок
        /// </summary>
        public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Произошла ошибка: {exception.Message}");
            return Task.CompletedTask;
        }
    }
}
