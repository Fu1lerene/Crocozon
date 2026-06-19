using Crocozon.Library.Domain.Abstractions;
using Crocozon.Library.ValueObjects;
using Crocozon.Library.ValueObjects.Helpers;

namespace Crocozon.Services.ItemsData.Domain.ValueObjects;

public readonly record struct ItemDataItemId : IAggregateId
{
    public Guid Value { get; }
    
    public ItemDataItemId(ItemId value)
        => Value = value.ToGuid();
    
    public ItemDataItemId(Guid value)
        => Value = value;
    
    public static implicit operator ItemId(ItemDataItemId value) => new(value.Value.ToLong());
    public static implicit operator ItemDataItemId(long value) => new(value);
}