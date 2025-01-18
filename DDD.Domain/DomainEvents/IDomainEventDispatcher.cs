namespace DDD.Domain.DomainEvents;

public interface IDomainEventDispatcher
{
    public void DispatchEvents();
}