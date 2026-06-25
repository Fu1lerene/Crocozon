namespace Crocozon.Library.EventStore.Abstractions;

public record EventsDataWriteRequest(Guid AggregateId, long ExpectedVersion, IReadOnlyCollection<EventData> Events);