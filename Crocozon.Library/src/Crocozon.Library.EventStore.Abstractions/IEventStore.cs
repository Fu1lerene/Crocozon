using Crocozon.Library.Domain.Abstractions;

namespace Crocozon.Library.EventStore.Abstractions;

public interface IEventStore
{
    Task SaveEventsAsync(Guid aggregateId, long expectedVersion, IReadOnlyCollection<EventsEnvelope> events, CancellationToken cancellationToken);
    Task<RecordedDomainEvents> ReadEventsAsync(Guid aggregateId, CancellationToken cancellationToken);
}