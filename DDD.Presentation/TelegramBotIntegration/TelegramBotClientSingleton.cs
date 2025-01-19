using Telegram.Bot;

namespace DDD.Presentation.TelegramBotIntegration;

public static class TelegramBotClientSingleton
{
    public static ITelegramBotClient? TelegramBotClient { get; set; } = null;
}
