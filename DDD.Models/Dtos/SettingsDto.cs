using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Models.Dtos
{
    public record SettingsDto(string TelegramId, TimeOnly TimeToSpendMessages, int WordAmount) : BaseDto;
}
