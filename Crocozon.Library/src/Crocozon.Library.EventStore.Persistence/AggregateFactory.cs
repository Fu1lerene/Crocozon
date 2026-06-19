using Crocozon.Library.Domain.Abstractions;
using Crocozon.Library.EventStore.Abstractions;

namespace Crocozon.Library.EventStore.Persistence;

public class AggregateFactory<TAggregate, TId>
    (Func<TId, IReadOnlyCollection<IDomainEvent>, TAggregate> factory) : IAggregateFactory<TAggregate, TId>
{
    public TAggregate Create(TId id, IReadOnlyCollection<IDomainEvent> events)
        => factory(id, events);
}