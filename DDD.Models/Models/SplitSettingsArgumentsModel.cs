namespace DDD.Models.Models;

public class SplitSettingsArgumentsModel
{
    public bool IsValid;
    public TimeOnly? TimeToSpendMessages;
    public int? WordAmount;

    public SplitSettingsArgumentsModel(bool isValid, TimeOnly? timeToSpendMessages, int? wordAmount)
    {
        IsValid = isValid;
        TimeToSpendMessages = timeToSpendMessages;
        WordAmount = wordAmount;
    }
}
