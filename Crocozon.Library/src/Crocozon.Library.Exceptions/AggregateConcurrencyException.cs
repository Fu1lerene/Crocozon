namespace Crocozon.Library.Exceptions;

public sealed class AggregateConcurrencyException(string message, Exception innerException, string detail) : Exception(message, innerException)
{
    public string Detail { get; init; } = detail;
}