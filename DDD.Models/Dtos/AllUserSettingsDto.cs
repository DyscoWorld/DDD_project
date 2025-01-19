using DDD.Models.Models;

namespace DDD.Models.Dtos;

public record AllUserSettingsDto(
    List<TelegramIdAndSettingsDto> IdAndSettingsDtos
) : BaseDto;

public record TelegramIdAndSettingsDto(
    string TelegramId,
    bool TrainedToday,
    Settings Settings
) : BaseDto;