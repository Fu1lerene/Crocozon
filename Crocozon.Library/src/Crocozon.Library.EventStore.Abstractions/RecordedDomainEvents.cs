using Crocozon.Library.Domain.Abstractions;

namespace Crocozon.Library.EventStore.Abstractions;

public record struct RecordedDomainEvents(Guid AggregateId, IReadOnlyCollection<EventsEnvelope> EventsData, long LastNumberVersion)
{
    public static RecordedDomainEvents Empty(Guid aggregateId) => new (aggregateId, [], -1);
}