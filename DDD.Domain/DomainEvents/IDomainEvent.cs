using DDD.Models.Dtos;

namespace DDD.Domain.DomainEvents;

public interface IDomainEvent<TRequest, TResponse>
    where TRequest : BaseDto
{
    public Task<TResponse> Handle(TRequest dto);
}
