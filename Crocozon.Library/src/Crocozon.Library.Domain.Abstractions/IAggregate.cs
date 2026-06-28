namespace Crocozon.Library.Domain.Abstractions;

public interface IAggregate<out TId>
    where TId : IAggregateId
{
    TId Id { get; }
    long Version { get; }
    IReadOnlyCollection<IDomainEvent> UncommittedEvents { get; }
    void ClearUncommittedEvents();
}