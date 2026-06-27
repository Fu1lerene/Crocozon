using Crocozon.Library.Metadata;

namespace Crocozon.Library.Domain.Abstractions;

public interface IAggregate<out TId>
    where TId : IAggregateId
{
    TId Id { get; }
    long Version { get; }
    IReadOnlyCollection<EventsEnvelope> UncommittedEvents { get; }
    void ClearUncommittedEvents();

    void SetEventsMetadata(EventMetadata eventMetadata);
}