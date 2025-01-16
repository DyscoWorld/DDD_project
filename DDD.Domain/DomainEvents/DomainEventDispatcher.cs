using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Domain.DomainEvents
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly List<IDomainEvent> _events;

        public DomainEventDispatcher()
        {
            _events = new List<IDomainEvent>();
        }

        public void AddEvent(IDomainEvent domainEvent)
        {
            _events.Add(domainEvent);
        }

        public void DispatchEvents()
        {
            foreach (var domainEvent in _events)
            {
                domainEvent.Handle();
            }

            _events.Clear();
        }
    }
}