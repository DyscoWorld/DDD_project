using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Domain.DomainEvents
{
    // Диспетчер событий
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IEnumerable<IDomainEvent> _events;

        public DomainEventDispatcher(IEnumerable<IDomainEvent> events)
        {
            _events = events.ToList();
        }

        public void DispatchEvents()
        {
            foreach (var domainEvent in _events)
            {
                domainEvent.Handle();
            }
        }
    }
}