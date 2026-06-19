using Crocozon.Library.Exceptions;
using Crocozon.Library.ValueObjects.Helpers;

namespace Crocozon.Library.ValueObjects;

public readonly record struct Money
{
    public decimal Value { get; }
    public Currency Currency { get; }

    public static Money Zero => new(0);

    public Money(decimal value, Currency currency = Currency.Rub)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value), ExceptionMessages.ValueCannotBeNegative);
        
        Value = value;
        Currency = currency;
    }
    
    public Money(decimal value, string currencyCode)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value), ExceptionMessages.ValueCannotBeNegative);
        
        Value = value;
        Currency = CurrencyExtensions.ToCurrencyEnum(currencyCode);
    }
}