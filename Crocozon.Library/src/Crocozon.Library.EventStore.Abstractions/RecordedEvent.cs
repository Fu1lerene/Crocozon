namespace Crocozon.Library.EventStore.Abstractions;

public sealed record RecordedEvent(
    Guid AggregateId,
    long Version,
    string EventType,
    byte[] Payload,
    byte[] Metadata,
    DateTimeOffset Created);
