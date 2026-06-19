using Crocozon.Library.Metadata;

namespace Crocozon.Library.Domain.Abstractions;

public interface IAggregate<out TId>
    where TId : IAggregateId
{
    TId Id { get; }
    long Version { get; }
    IReadOnlyCollection<EventsEnvelope> UncommitedEvents { get; }
    void ClearUncommitedEvents();

    void SetEventsMetadata(EventMetadata eventMetadata);
}