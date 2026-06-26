using Crocozon.Library.Domain.Abstractions;
using Crocozon.Library.EventStore.Abstractions;
using Crocozon.Library.EventStore.Abstractions.Processing;

namespace Crocozon.Library.EventStore.Processing;

public class AggregateProcessor<TAggregate, TId>(IRepository<TAggregate, TId> repository) : IAggregateProcessor<TAggregate, TId>
where TAggregate : IAggregate<TId>
where TId : IAggregateId
{
    public IAggregateProcessingBuilder<TAggregate, TId, TRequest> For<TRequest>(IEnumerable<TRequest> requests, Func<TRequest, TId> idSelector)
        => new AggregateProcessingBuilder<TAggregate, TId, TRequest>(repository, requests, idSelector);
}