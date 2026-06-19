using Crocozon.Library.Extensions.Protobuf;
using Crocozon.Services.ItemsData.Domain.Events.ValueObjects;

namespace Crocozon.Services.ItemsData.Domain.Events.Protobuf;

public static class MappingExtensions
{
    public static ItemCreatedProto ToProto(this ItemCreated domain)
        => new()
        {
            ItemId = domain.ItemId,
            Name = domain.Name.ToProto(),
            BasePrice = domain.BasePrice.ToProto()
        };
    
    public static ItemCreated ToDomain(this ItemCreatedProto proto)
        => new(proto.ItemId, new ItemName(proto.Name), proto.BasePrice.ToDomain());
    
    public static ItemNameChangedProto ToProto(this ItemNameChanged domain)
        => new()
        {
            ItemId = domain.ItemId,
            Name = domain.Name.ToProto()
        };
    
    public static ItemNameChanged ToDomain(this ItemNameChangedProto proto)
        => new(proto.ItemId, new ItemName(proto.Name));

    private static string ToProto(this ItemName name)
        => string.IsNullOrWhiteSpace(name.Value) ? string.Empty : name.Value;
}