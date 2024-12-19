using DDD.Models.Data;
using DDD.Models.Models;
using DDD.Models.Repositories;
using MongoDB.Driver;

namespace DDD.Models.Services
{
    public class WordService
    {
        private readonly WordRepository _wordRepository;
        private readonly UserRepository _userRepository;

        public WordService(WordRepository wordRepository, UserRepository userRepository)
        {
            _wordRepository = wordRepository;
            _userRepository = userRepository;
        }

        public async Task AddWordAsync(string telegramId, string name, string translation, string definition, int rank, int timesRepeated, bool isLearned)
        {
            var user = await _userRepository.GetUserAsync(telegramId) ?? new User { TelegramId = telegramId, LastActivity = DateTime.UtcNow };
            if (user.Id == null)
            {
                await _userRepository.AddUserAsync(user);
            }

            var word = new Word
            {
                Name = name,
                Translation = translation,
                Definition = definition,
                Rank = rank,
                TimesRepeated = timesRepeated,
                IsLearned = isLearned,
                UserId = user.Id
            };

            await _wordRepository.AddWordAsync(word);
            user.WordIds.Add(word.Id);
            await _userRepository.UpdateUserAsync(user);
        }

        public async Task<List<Word>> GetWordsAsync(string telegramId)
        {
            var user = await _userRepository.GetUserAsync(telegramId);
            if (user == null)
            {
                return new List<Word>();
            }

            return await _wordRepository.GetWordsAsync(user.Id);
        }

        public async Task LearnWordAsync(string telegramId, string wordId)
        {
            var word = await _wordRepository.GetWordAsync(wordId);
            if (word != null)
            {
                word.TimesRepeated++;
                await _wordRepository.UpdateWordAsync(word);
            }
        }
    }

}
