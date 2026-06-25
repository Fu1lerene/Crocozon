namespace Crocozon.Library.Domain.Abstractions;

public record EventsWriteRequest(Guid AggregateId, long ExpectedVersion, IReadOnlyCollection<EventsEnvelope> Events);