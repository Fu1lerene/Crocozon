namespace Crocozon.Library.EventStore.Abstractions;

public interface IEventWriter
{
    Task WriteAsync(EventsDataWriteRequest request, CancellationToken cancellationToken);
    Task WriteAsync(IReadOnlyCollection<EventsDataWriteRequest> requests, CancellationToken cancellationToken);
}