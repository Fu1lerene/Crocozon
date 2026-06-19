namespace Crocozon.Library.EventStore.Abstractions;

public interface IEventWriter
{
    Task WriteAsync(Guid aggregateId, long expectedVersion, IReadOnlyCollection<EventData> events, CancellationToken cancellationToken);
}