using Crocozon.Library.Domain.Abstractions;
using Crocozon.Library.EventStore.Abstractions;
using Crocozon.Library.EventStore.Protobuf.Extensions;
using Crocozon.Library.Exceptions;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Crocozon.Library.EventStore.Protobuf;

public class EventSerializer(
    Dictionary<Type, (MessageDescriptor, Func<IDomainEvent, byte[]>)> mapDomainToProto,
    Dictionary<string, Func<byte[], IDomainEvent>> mapProtoToDomain)
    : IEventSerializer
{
    private readonly Dictionary<Type, (MessageDescriptor ProtoDescriptor, Func<IDomainEvent, byte[]> ToByteArray)>
        _mapDomainToProto = new(mapDomainToProto);

    private readonly Dictionary<string, Func<byte[], IDomainEvent>> _mapProtoToDomain = new(mapProtoToDomain);

    public EventData Serialize(EventsEnvelope eventEnvelope)
    {
        ArgumentNullException.ThrowIfNull(eventEnvelope);
        var @event = eventEnvelope.Event;
        var metadata = eventEnvelope.Metadata;

        var eventType = @event.GetType();

        if (!_mapDomainToProto.TryGetValue(eventType, out var serializer))
            throw new ArgumentException(ExceptionMessages.EventTypeNotFound(eventType.ToString()), nameof(eventEnvelope));
        
        var data = serializer.ToByteArray(@event);

        return new EventData(serializer.ProtoDescriptor.FullName, data, metadata.ToProto().ToByteArray());
    }

    public EventsEnvelope Deserialize(string eventType, byte[] data, byte[] metadata)
    {
        if (!_mapProtoToDomain.TryGetValue(eventType, out var deserialize))
            throw new ArgumentException(ExceptionMessages.EventTypeNotFound(eventType), nameof(eventType));

        var domainEvent = deserialize(data);
        
        return new EventsEnvelope(domainEvent, MetadataProtoExtensions.Deserialize(metadata));
    }
}