namespace DDD.Models.Models;

public class SplitAddWordArgumentsModel
{
    public bool IsValid;
    public string Word;
    public string Translation;
    public string Description;

    public SplitAddWordArgumentsModel(bool isValid, string word, string translation, string description)
    {
        IsValid = isValid;
        Word = word;
        Translation = translation;
        Description = description;
    }
}
