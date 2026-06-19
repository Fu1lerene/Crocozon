using Crocozon.Library.Domain.Abstractions;

namespace Crocozon.Library.EventStore.Abstractions;

public interface IEventSerializer
{
    EventData Serialize(EventsEnvelope eventEnvelope);
    EventsEnvelope Deserialize(string eventType, byte[] data, byte[] metadata);
}