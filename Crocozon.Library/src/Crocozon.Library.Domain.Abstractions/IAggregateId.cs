namespace Crocozon.Library.Domain.Abstractions;

public interface IAggregateId
{
    Guid Value { get; }
}