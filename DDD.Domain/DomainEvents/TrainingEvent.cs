﻿using DDD.Infrastructure.Helpers;
using DDD.Infrastructure.Repositories.Interfaces;
using DDD.Models.Dtos;
using DDD.Models.Models;

namespace DDD.Domain.DomainEvents;

public class TrainingEvent(IUserRepository userRepository) : IDomainEvent<GetTrainingWordsRequestDto, GetTrainingWordsResponseDto>
{
    public async Task<GetTrainingWordsResponseDto> Handle(GetTrainingWordsRequestDto dto)
    {
        var words = await userRepository.GetAllPersonalWords(dto.TelegramId);
        var settings = await userRepository.GetSettings(dto.TelegramId);

        words = words
            .Where(x => !x.IsLearned)
            .RandomPermutation()
            .Take(words.Count < settings.WordAmount ? words.Count : settings.WordAmount)
            .ToList();
   
        return new GetTrainingWordsResponseDto(dto.TelegramId, words.Select(MapWordToTrainingWord).ToList());
    }

    private TrainingWordDto MapWordToTrainingWord(Word word)
    {
        return new TrainingWordDto(word.Name, word.Translation, word.Translation);
    }
}
