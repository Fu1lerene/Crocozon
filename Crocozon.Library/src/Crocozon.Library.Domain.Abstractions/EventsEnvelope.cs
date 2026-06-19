using Crocozon.Library.Metadata;

namespace Crocozon.Library.Domain.Abstractions;

public sealed record EventsEnvelope(IDomainEvent Event, EventMetadata Metadata);