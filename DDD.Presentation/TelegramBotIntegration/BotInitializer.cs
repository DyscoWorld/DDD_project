using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using DDD.Presentation.Handlers;

namespace DDD.Presentation.TelegramBotIntegration
{
    /// <summary>
    /// Класс для инициализации бота
    /// </summary>
    public class BotInitializer
    {
        private readonly ITelegramBotClient _botClient;
        private readonly UpdateHandler _updateHandler;
        private readonly CancellationTokenSource _cancellationTokenSource;

        /// <summary>
        /// Конструктор BotInitializer
        /// </summary>
        /// <param name="updateHandler">Экземпляр UpdateHandler для обработки сообщений</param>
        public BotInitializer(UpdateHandler updateHandler)
        {
            _botClient = new TelegramBotClient(BotConfiguration.GetApiKey());
            _updateHandler = updateHandler ?? throw new ArgumentNullException(nameof(updateHandler));
            _cancellationTokenSource = new CancellationTokenSource();

            TelegramBotClientSingleton.TelegramBotClient = _botClient;
        }

        /// <summary>
        /// Асинхронный запуск бота
        /// </summary>
        public Task StartBotAsync()
        {
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };

            Console.WriteLine("Бот запущен.");
            
            _botClient.StartReceiving(
                updateHandler: _updateHandler.HandleUpdateMessageAsync,
                errorHandler: _updateHandler.HandleErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: _cancellationTokenSource.Token
            );

            return Task.CompletedTask; 
        }

        /// <summary>
        /// Остановка бота
        /// </summary>
        public void StopBot()
        {
            Console.WriteLine("Бот останавливается...");
            _cancellationTokenSource.Cancel();
        }
    }
}
