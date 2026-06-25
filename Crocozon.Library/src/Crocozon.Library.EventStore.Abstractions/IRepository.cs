using Crocozon.Library.Domain.Abstractions;

namespace Crocozon.Library.EventStore.Abstractions;

public interface IRepository<TAggregate, TId>
    where TAggregate : IAggregate<TId>
    where TId : IAggregateId
{
    Task SaveAsync(TAggregate aggregate, CancellationToken cancellationToken);
    Task SaveAsync(IReadOnlyCollection<TAggregate> aggregates, CancellationToken cancellationToken);
    Task<TAggregate?> GetAsync(TId id, CancellationToken cancellationToken);
    IAsyncEnumerable<(TId, TAggregate?)> GetAsync(IReadOnlyCollection<TId> ids, CancellationToken cancellationToken);
}