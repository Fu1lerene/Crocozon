using Crocozon.Library.Domain.Abstractions;

namespace Crocozon.Library.EventStore.Abstractions;

public interface IAggregateFactory<out TAggregate, in TId>
    where TId : IAggregateId
{
    TAggregate Create(TId id, IReadOnlyCollection<IDomainEvent> events);
}