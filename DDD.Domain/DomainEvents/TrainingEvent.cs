using DDD.Infrastructure.Repositories.Interfaces;
using DDD.Models.Dtos;
using DDD.Models.Models;

namespace DDD.Domain.DomainEvents;

public class TrainingEvent(IUserRepository userRepository) : IDomainEvent<GetTrainingWordsRequestDto, GetTrainingWordsResponseDto>
{
    public async Task<GetTrainingWordsResponseDto> Handle(GetTrainingWordsRequestDto dto)
    {
        var words = await userRepository.GetAllPersonalWords(dto.TelegramId);
        words = words.Where(x => !x.IsLearned).ToList();
        var settings = await userRepository.GetSettings(dto.TelegramId);

        if (words.Count > settings.WordAmount)
        {
            var random = new Random();

            var randomWords = new Word[settings.WordAmount];
            for (var i = 0; i < settings.WordAmount; i++)
            {
                randomWords[i] = words[random.Next(words.Count)];
            }

            words = randomWords.ToList();
        }
   
        return new GetTrainingWordsResponseDto(dto.TelegramId, words.Select(MapWordToTrainingWord).ToList());
    }

    private TrainingWordDto MapWordToTrainingWord(Word word)
    {
        return new TrainingWordDto(word.Name, word.Translation, word.Translation);
    }
}
