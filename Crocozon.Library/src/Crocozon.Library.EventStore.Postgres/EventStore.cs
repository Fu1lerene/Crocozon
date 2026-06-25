using System.Runtime.CompilerServices;
using Crocozon.Library.Domain.Abstractions;
using Crocozon.Library.EventStore.Abstractions;

namespace Crocozon.Library.EventStore.Postgres;

public class EventStore(IEventSerializer serializer, IEventWriter eventWriter, IEventReader eventReader) : IEventStore
{
    public async Task SaveEventsAsync(EventsWriteRequest request, CancellationToken cancellationToken)
    {
        var writeRequest = GetEventsWriteRequest(request.AggregateId, request.ExpectedVersion, request.Events);

        await eventWriter.WriteAsync(writeRequest, cancellationToken);
    }
    
    public async Task SaveEventsAsync(IReadOnlyCollection<EventsWriteRequest> requests, CancellationToken cancellationToken)
    {
        var writeRequests = requests
            .Select(x => GetEventsWriteRequest(x.AggregateId, x.ExpectedVersion, x.Events))
            .ToArray();

        await eventWriter.WriteAsync(writeRequests, cancellationToken);
    }

    public async Task<RecordedDomainEvents> ReadEventsAsync(Guid aggregateId, CancellationToken cancellationToken)
    {
        var recordedEvents = await eventReader.ReadAsync(aggregateId, cancellationToken);

        return GetDeserializedEvents(aggregateId, recordedEvents);
    }
    
    public async IAsyncEnumerable<RecordedDomainEvents> ReadEventsAsync(IReadOnlyCollection<Guid> aggregateIds, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var sortedIds = aggregateIds.Distinct().Order().ToArray();

        await foreach (var deserializedEvents in ReadRecordedEventsAsync(sortedIds, cancellationToken)) 
            yield return deserializedEvents;
    }
    
    private async IAsyncEnumerable<RecordedDomainEvents> ReadRecordedEventsAsync(Guid[] sortedIds, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await using var enumerator = eventReader.ReadAsync(sortedIds, cancellationToken)
            .GetAsyncEnumerator(cancellationToken);

        var hasEvent = await enumerator.MoveNextAsync();
        var currentEvents = new List<RecordedEvent>();

        foreach (var aggregateId in sortedIds)
        {
            currentEvents.Clear();

            while (hasEvent && enumerator.Current.AggregateId == aggregateId)
            {
                currentEvents.Add(enumerator.Current);
                hasEvent = await enumerator.MoveNextAsync();
            }

            yield return GetDeserializedEvents(aggregateId, currentEvents);
        }
    }
    
    private RecordedDomainEvents GetDeserializedEvents(Guid aggregateId, IReadOnlyCollection<RecordedEvent> events)
    {
        if (events.Count == 0)
            return RecordedDomainEvents.Empty(aggregateId);

        var eventsEnvelopes = events.Select(DeserializeEvents).ToArray();
        var lastNumberVersion = events.Last().Version;

        return new RecordedDomainEvents(aggregateId, eventsEnvelopes, lastNumberVersion);
    }

    private EventsDataWriteRequest GetEventsWriteRequest(Guid aggregateId, long expectedVersion, IReadOnlyCollection<EventsEnvelope> events)
        => new(aggregateId, expectedVersion, [.. events.Select(serializer.Serialize)]);

    private EventsEnvelope DeserializeEvents(RecordedEvent @event)
        => serializer.Deserialize(@event.EventType, @event.Payload, @event.Metadata);
}