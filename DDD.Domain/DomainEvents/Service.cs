using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Domain.DomainEvents
{
    public class Service
    {
        private readonly DomainEventDispatcher _dispatcher;

        public Service(DomainEventDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public void DoSomething()
        {
            _dispatcher.DispatchEvents();
        }
    }
}
