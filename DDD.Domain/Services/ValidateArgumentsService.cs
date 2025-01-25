using DDD.Infrastructure.Helpers;
using DDD.Infrastructure.Repositories.Interfaces;
using DDD.Models.Models;

namespace DDD.Domain.Services;

/// <inheritdoc />
public class ValidateArgumentsService : IValidateArgumentsService
{
    /// <inheritdoc />
    public SplitAddWordArgumentsModel ValidateAndSplitAddWordArguments(string? arguments)
    {
        if (arguments is null)
            return new SplitAddWordArgumentsModel(false, null!, null!, null!);

        var splittedParts = arguments.Split(':');

        return splittedParts.Length != 3
            ? new SplitAddWordArgumentsModel(false, null!, null!, null!)
            : new SplitAddWordArgumentsModel(true, splittedParts[0], splittedParts[1], splittedParts[2]);
    }
    
    /// <inheritdoc />
    public SplitSettingsArgumentsModel ValidateAndSplitSettingsArguments(string? arguments)
    {
        var notValidDto = new SplitSettingsArgumentsModel(false, null!, null!);

        if (arguments is null)
            return notValidDto;

        var indexOfSplitColon = arguments.LastIndexOf(':');
        if (indexOfSplitColon == -1)
            return notValidDto;

        var stringedTime = arguments[..indexOfSplitColon];
        var stringedWordsAmount = arguments[(indexOfSplitColon + 1)..];

        if (!TimeOnly.TryParse(stringedTime, out var spendMessageTimeOnly) || !int.TryParse(stringedWordsAmount, out var wordsAmount))
            return notValidDto;

        return new SplitSettingsArgumentsModel(true, spendMessageTimeOnly.ToUtc(), wordsAmount);
    }
}