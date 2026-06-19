using Crocozon.Library.Exceptions;
using Crocozon.Library.ValueObjects.Helpers;

namespace Crocozon.Library.ValueObjects;

public readonly record struct ItemId
{
    public long Value { get; init; }
    
    public ItemId(long value)
    {
        if (value <= 0)
            throw new ArgumentOutOfRangeException(nameof(value), ExceptionMessages.ValueMustBePositive);
        
        Value = value;
    }

    public Guid ToGuid() => Value.ToGuid();
    
    public static implicit operator long(ItemId value) => value.Value;
    
    public static implicit operator ItemId(long value) => new(value);
}