using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Domain.DomainEvents
{
    public interface IDomainEvent
    {
        void Handle();
    }

    // Добавление слова
    public class AddSingleWord : IDomainEvent
    {
        public void Handle()
        {
            // Пустая реализация
        }
    }

    // Добавление внешних слов
    public class AddExternalWords : IDomainEvent
    {
        public void Handle()
        {
            // Пустая реализация
        }
    }

    // Список слов
    public class ListWords : IDomainEvent
    {
        public void Handle()
        {
            // Пустая реализация
        }
    }

    // Список слов пользователя
    public class ListMyWords : IDomainEvent
    {
        public void Handle()
        {
            // Пустая реализация
        }
    }

    // Слово изучено
    public class LearnWord : IDomainEvent
    {
        public void Handle()
        {
            // Пустая реализация
        }
    }

    // Диспетчер событий
    public static class DomainEventDispatcher
    {
        private static readonly List<IDomainEvent> _events = new List<IDomainEvent>();

        public static void AddEvent(IDomainEvent domainEvent)
        {
            _events.Add(domainEvent);
        }

        public static void DispatchEvents()
        {
            foreach (var domainEvent in _events)
            {
                domainEvent.Handle();
            }

            _events.Clear();
        }
    }
}
