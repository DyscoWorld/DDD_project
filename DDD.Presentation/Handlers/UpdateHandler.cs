using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DDD.Presentation.Handlers
{
    /// <summary>
    /// Класс handler для сообщений
    /// </summary>
    public static class UpdateHandler
    {
        public static async Task HandleUpdateMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.Message && update.Message is not null)
            {
                var message = update.Message;

                Console.WriteLine($"Получено сообщение от {message.Chat.Id}: {message.Text}");

                if (message.Text is not null)
                {
                    string response = message.Text.ToLower() switch
                    {
                        "/start" => "Привет! Я ваш бот. Чем могу помочь?",
                        "/help" => "Вот список доступных команд:\n/start - запуск\n/help - помощь",
                        _ => "Извините, я не понимаю эту команду."
                    };

                    await botClient.SendMessage(
                        chatId: message.Chat.Id,
                        text: response,
                        cancellationToken: cancellationToken
                    );
                }
            }
        }

        /// <summary>
        /// Класс handler для ошибок
        /// </summary>
        public static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Произошла ошибка: {exception.Message}");
            return Task.CompletedTask;
        }
    }
}