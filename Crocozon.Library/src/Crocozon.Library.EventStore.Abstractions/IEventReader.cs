namespace Crocozon.Library.EventStore.Abstractions;

public interface IEventReader
{
    Task<IReadOnlyCollection<RecordedEvent>> ReadAsync(Guid aggregateId, CancellationToken cancellationToken);
    IAsyncEnumerable<RecordedEvent> ReadAsync(IReadOnlyCollection<Guid> aggregateIds, CancellationToken cancellationToken);
}