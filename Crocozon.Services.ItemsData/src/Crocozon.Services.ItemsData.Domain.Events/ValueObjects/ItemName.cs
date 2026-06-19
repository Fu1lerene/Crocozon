using Crocozon.Library.Exceptions;

namespace Crocozon.Services.ItemsData.Domain.Events.ValueObjects;

public readonly record struct ItemName
{
    public string Value { get; }

    private const short MaxLengthName = 50;

    public static ItemName DefaultName => new("New item");

    public ItemName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException(ExceptionMessages.ValueCannotBeNullOrWhitespace, nameof(value));
        if (value.Length > MaxLengthName)
            throw new ArgumentException(ExceptionMessages.ItemNameTooLong(MaxLengthName), nameof(value));
        
        Value = value;
    }
}