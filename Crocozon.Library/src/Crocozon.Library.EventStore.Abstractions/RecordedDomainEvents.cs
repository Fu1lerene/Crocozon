using Crocozon.Library.Domain.Abstractions;

namespace Crocozon.Library.EventStore.Abstractions;

public record struct RecordedDomainEvents(IReadOnlyCollection<EventsEnvelope> EventsData, long LastNumberVersion)
{
    public static RecordedDomainEvents Empty => new ([], -1);
}