using Crocozon.Library.ValueObjects;
using Crocozon.Library.ValueObjects.Helpers;
using MoneyProto = Google.Type.Money;

namespace Crocozon.Library.Extensions.Protobuf;

public static class MoneyExtensions
{
    public static MoneyProto ToProto(this Money domain)
        => new()
        {
            DecimalValue = domain.Value,
            CurrencyCode = domain.Currency.ToUpperString()
        };

    public static Money ToDomain(this MoneyProto proto)
    {
        return new Money(proto.DecimalValue, CurrencyExtensions.ToCurrencyEnum(proto.CurrencyCode));
    }
}