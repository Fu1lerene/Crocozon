using Crocozon.Library.Domain.Abstractions;

namespace Crocozon.Library.EventStore.Abstractions;

public interface IRepository<TAggregate, in TId>
    where TAggregate : IAggregate<TId>
    where TId : IAggregateId
{
    Task SaveAsync(TAggregate aggregate, CancellationToken cancellationToken);
    Task<TAggregate?> GetAsync(TId id, CancellationToken cancellationToken);
}