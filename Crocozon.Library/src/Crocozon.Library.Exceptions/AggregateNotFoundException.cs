namespace Crocozon.Library.Exceptions;

public sealed class AggregateNotFoundException(string message) : Exception(message);