using Crocozon.Library.Exceptions;

namespace Crocozon.Library.ValueObjects.Helpers;

public static class CurrencyExtensions
{
    public static string ToUpperString(this Currency currency) => currency switch
    {
        _ => currency.ToString().ToUpperInvariant()
    };
    
    public static Currency ToCurrencyEnum(string currency)
    {
        if (!Enum.TryParse<Currency>(currency, true, out var currencyCode))
            throw new ArgumentException(ExceptionMessages.UnknownCurrencyCode, nameof(currency));
        
        return currencyCode;
    }
}