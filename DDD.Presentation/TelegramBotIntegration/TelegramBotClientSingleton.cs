using Telegram.Bot;

namespace DDD.Presentation.TelegramBotIntegration;

public class GetTelegramBotClientService
{
    public ITelegramBotClient TelegramBotClient { get; set; } = null!;
}
