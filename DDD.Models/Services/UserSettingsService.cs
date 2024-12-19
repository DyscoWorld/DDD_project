using DDD.Models.Models;
using DDD.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Models.Services
{
    public class UserSettingsService
    {
        private readonly UserSettingsRepository _userSettingsRepository;
        private readonly UserRepository _userRepository;

        public UserSettingsService(UserSettingsRepository userSettingsRepository, UserRepository userRepository)
        {
            _userSettingsRepository = userSettingsRepository;
            _userRepository = userRepository;
        }

        public async Task<UserSettings> GetUserSettingsAsync(string telegramId)
        {
            var user = await _userRepository.GetUserAsync(telegramId);
            if (user == null)
            {
                return null;
            }

            return await _userSettingsRepository.GetUserSettingsAsync(user.Id);
        }

        public async Task UpdateUserSettingsAsync(string telegramId, string timeToSpendMessages, int wordAmount)
        {
            var user = await _userRepository.GetUserAsync(telegramId);
            if (user == null)
            {
                return;
            }

            var settings = new UserSettings
            {
                TimeToSpendMessages = timeToSpendMessages,
                WordAmount = wordAmount
            };

            await _userSettingsRepository.UpdateUserSettingsAsync(user.Id, settings);
        }
    }

}
