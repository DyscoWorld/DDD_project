using DDD.Presentation.Handlers;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace DDD.Presentation.TelegramBotIntegration
{
    /// <summary>
    /// Класс для инициализации бота
    /// </summary>
    public static class BotInitializer
    {
        private static readonly TelegramBotClient _botClient;
        private static readonly CancellationTokenSource _cancellationTokenSource;

        static BotInitializer()
        {
            _botClient = new TelegramBotClient(BotConfiguration.GetApiKey());
            _cancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// Асинхронный запуск бота
        /// </summary>
        public static async Task StartBotAsync()
        {
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = [UpdateType.Message]
            };

            Console.WriteLine("Бот запущен.");

            _botClient.StartReceiving(
                UpdateHandler.HandleUpdateMessageAsync,
                UpdateHandler.HandleErrorAsync,
                receiverOptions,
                cancellationToken: _cancellationTokenSource.Token
            );

            // Запуск бота в фоновом режиме
            await Task.CompletedTask;
        }

        /// <summary>
        /// Остановка бота
        /// </summary>
        public static void StopBot()
        {
            Console.WriteLine("Бот останавливается...");
            _cancellationTokenSource.Cancel();
        }
    }
}