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
        /// Запуск бота
        /// </summary>
        public static void StartBot()
        {
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = [UpdateType.Message]
            };

            _botClient.StartReceiving(
                UpdateHandler.HandleUpdateMessageAsync,
                UpdateHandler.HandleErrorAsync,
                receiverOptions,
                cancellationToken: _cancellationTokenSource.Token
            );

            Console.WriteLine("Бот запущен. Нажмите любую клавишу для завершения.");
            Console.ReadKey();
            StopBot();
        }

        /// <summary>
        /// Остановка бота
        /// </summary>
        public static void StopBot()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}