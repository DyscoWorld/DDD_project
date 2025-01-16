using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.DomainEvents;
using DDD.Infrastructure.Data;
using DDD.Presentation.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace DDD.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WordController : ControllerBase
    {
        private readonly DomainEventDispatcher _dispatcher;
        private readonly IMongoCollection<WordEntity> _wordsCollection;
        private readonly IMongoCollection<SettingsEntity> _settingsCollection;

        public WordController(
            DomainEventDispatcher dispatcher,
            IMongoCollection<WordEntity> wordsCollection,
            IMongoCollection<SettingsEntity> settingsCollection)
        {
            _dispatcher = dispatcher;
            _wordsCollection = wordsCollection;
            _settingsCollection = settingsCollection;
        }

        // Endpoint: /add
        [HttpPost("add")]
        public IActionResult Add([FromBody] AddWordRequest request, [FromServices] IMongoCollection<WordEntity> wordsCollection)
        {
            var domainEvent = new AddSingleWord(wordsCollection)
            {
                Word = request.Word
            };

            _dispatcher.AddEvent(domainEvent);
            _dispatcher.DispatchEvents();

            return Ok("Слово добавлено");
        }

        // Endpoint: /list
        [HttpGet("list")]
        public IActionResult List()
        {
            var domainEvent = new ListWords(_wordsCollection);
            _dispatcher.AddEvent(domainEvent);
            _dispatcher.DispatchEvents();

            return Ok(domainEvent.Result);
        }

        // Endpoint: /learn
        [HttpPost("learn")]
        public IActionResult Learn([FromBody] LearnWordRequest request)
        {
            var domainEvent = new LearnWord(_wordsCollection)
            {
                WordId = request.WordId
            };

            _dispatcher.AddEvent(domainEvent);
            _dispatcher.DispatchEvents();

            return Ok("Слово отмечено как изученное");
        }

        // Endpoint: /settings
        [HttpPost("settings")]
        public IActionResult Settings([FromBody] SettingsRequest request)
        {
            var domainEvent = new SettingsEvent(_settingsCollection)
            {
                Settings = request.Settings
            };

            _dispatcher.AddEvent(domainEvent);
            _dispatcher.DispatchEvents();

            return Ok("Настройки обновлены");
        }
    }
}
