using Crocozon.Library.EventStore.Abstractions;
using Crocozon.Library.EventStore.Protobuf;

namespace Crocozon.Services.ItemsData.Domain.Events.Protobuf;

public static class DomainEventSerializer
{
    public static IEventSerializer Instance { get; }

    static DomainEventSerializer()
    {
        var builder = new EventSerializerBuilder(EventsReflection.Descriptor.MessageTypes)
            .CreateMap<ItemCreated, ItemCreatedProto>(d => d.ToProto(), p => p.ToDomain())
            .CreateMap<ItemNameChanged, ItemNameChangedProto>(d => d.ToProto(), p => p.ToDomain());
        
        Instance = builder.Build();
    }
}