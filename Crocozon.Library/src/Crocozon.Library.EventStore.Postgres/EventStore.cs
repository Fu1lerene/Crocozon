using Crocozon.Library.Domain.Abstractions;
using Crocozon.Library.EventStore.Abstractions;

namespace Crocozon.Library.EventStore.Postgres;

public class EventStore(IEventSerializer serializer, IEventWriter eventWriter, IEventReader eventReader) : IEventStore
{
    public async Task SaveEventsAsync(Guid aggregateId, long expectedVersion, IReadOnlyCollection<EventsEnvelope> events, CancellationToken cancellationToken)
    {
        var serializedEvents = events.Select(serializer.Serialize).ToArray();
        await eventWriter.WriteAsync(aggregateId, expectedVersion, serializedEvents, cancellationToken);
    }

    public async Task<RecordedDomainEvents> ReadEventsAsync(Guid aggregateId, CancellationToken cancellationToken)
    {
        var recordedEvents = await eventReader.ReadAsync(aggregateId, cancellationToken);
        if (recordedEvents.Count == 0)
            return RecordedDomainEvents.Empty;

        var eventsEnvelope = recordedEvents.Select(DeserializeEvents).ToArray();
        var lastNumberVersion = recordedEvents.Last().Version;
        
        return new RecordedDomainEvents(eventsEnvelope, lastNumberVersion);
    }

    private EventsEnvelope DeserializeEvents(RecordedEvent @event)
        => serializer.Deserialize(@event.EventType, @event.Payload, @event.Metadata);
}