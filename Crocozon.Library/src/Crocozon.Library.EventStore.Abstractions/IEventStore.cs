using Crocozon.Library.Domain.Abstractions;

namespace Crocozon.Library.EventStore.Abstractions;

public interface IEventStore
{
    Task SaveEventsAsync(EventsWriteRequest request, CancellationToken cancellationToken);
    Task SaveEventsAsync(IReadOnlyCollection<EventsWriteRequest> requests, CancellationToken cancellationToken);
    Task<RecordedDomainEvents> ReadEventsAsync(Guid aggregateId, CancellationToken cancellationToken);
    IAsyncEnumerable<RecordedDomainEvents> ReadEventsAsync(IReadOnlyCollection<Guid> aggregateIds, CancellationToken cancellationToken);
}