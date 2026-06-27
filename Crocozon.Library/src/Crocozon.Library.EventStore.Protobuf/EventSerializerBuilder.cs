using Crocozon.Library.Domain.Abstractions;
using Crocozon.Library.EventStore.Abstractions;
using Crocozon.Library.Exceptions;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Crocozon.Library.EventStore.Protobuf;

public class EventSerializerBuilder(IEnumerable<MessageDescriptor> protoDescriptors)
{
    private readonly IReadOnlyDictionary<Type, MessageDescriptor> _protoDescriptors = protoDescriptors.ToDictionary(x => x.ClrType);
    private readonly Dictionary<Type, (MessageDescriptor, Func<IDomainEvent, byte[]>)> _mapDomainToProto = [];
    private readonly Dictionary<string, Func<byte[], IDomainEvent>> _mapProtoToDomain = [];

    public IEventSerializer Build() => new EventSerializer(_mapDomainToProto, _mapProtoToDomain);

    public EventSerializerBuilder CreateMap<TDomain, TProto>(
        Func<TDomain, TProto> toProto,
        Func<TProto, TDomain> toDomain)
        where TDomain : IDomainEvent
        where TProto : IMessage
    {
        var domainType = typeof(TDomain);
        var protoType = typeof(TProto);

        if (!_protoDescriptors.TryGetValue(protoType, out var descriptor))
            throw new ArgumentException(ExceptionMessages.DescriptorNotFound(protoType.FullName!), nameof(toProto));

        if (!_mapDomainToProto.TryAdd(domainType, (descriptor, domain => toProto((TDomain)domain).ToByteArray())))
            throw new ArgumentException(ExceptionMessages.DomainEventAlreadyMapped(domainType.FullName!), nameof(toProto));

        if (!_mapProtoToDomain.TryAdd(descriptor.FullName, bytes => toDomain((TProto)descriptor.Parser.ParseFrom(bytes))))
            throw new ArgumentException(ExceptionMessages.ProtoMessageAlreadyMapped(descriptor.FullName), nameof(toDomain));

        return this;
    }
}