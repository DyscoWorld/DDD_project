namespace DDD.Models.Models;

public class AllUserSettingsModel
{
    public List<TelegramIdAndSettingsModel> IdAndSettings;
}

public class TelegramIdAndSettingsModel
{
    public string TelegramId;
    public bool TrainedToday;
    public Settings Settings;
}
