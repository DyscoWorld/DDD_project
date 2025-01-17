using DDD.Infrastructure.Repositories.Interfaces;
using DDD.Models.Models;
using Microsoft.Extensions.Logging;

namespace DDD.Domain.Services
{
    public class BotCommandService : IBotCommandService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<BotCommandService> _logger;

        public BotCommandService(IUserRepository userRepository, ILogger<BotCommandService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;   
        }

        public async Task<string> HandleStartCommandAsync(string telegramId)
        {
            var user = await _userRepository.GetByTelegramIdAsync(telegramId);
            if (user == null)
            {
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

            return "Привет! Я ваш бот. Чем могу помочь?";
        }

        public async Task<string> HandleAddWordCommandAsync(string telegramId, string commandText)
        {
            var parts = commandText.Split(' ');
            if (parts.Length < 5)
            {
                return "Формат команды: /addword <слово> <перевод> <описание> <ранг>";
            }

            var word = parts[1];
            var translation = parts[2];
            var definition = parts[3];

            if (!int.TryParse(parts[4], out int rank))
            {
                return "Ранг должен быть числом!";
            }

            // Ищем пользователя
            var user = await _userRepository.GetByTelegramIdAsync(telegramId);

            if (user is null)
            {
                await HandleStartCommandAsync(telegramId);
            }
            
            user = await _userRepository.GetByTelegramIdAsync(telegramId);

            // Добавляем слово в список пользователя
            var newWord = new Word
            {
                Name = word,
                Translation = translation,
                Definition = definition,
                Rank = rank,
                IsLearned = false
            };

            user.PersonalWords.Add(newWord);
            await _userRepository.UpdateAsync(user);

            _logger.LogInformation($"Добавлено слово '{word}' пользователю {telegramId}");
            return $"Слово '{word}' успешно добавлено!";
        }
        
        public Task<string> HandleHelpCommandAsync()
        {
            return Task.FromResult("Вот список доступных команд:\n/start - запуск\n/help - помощь");
        }

        public Task<string> HandleUnknownCommandAsync()
        {
            return Task.FromResult("Извините, я не понимаю эту команду.");
        }
    }
}
