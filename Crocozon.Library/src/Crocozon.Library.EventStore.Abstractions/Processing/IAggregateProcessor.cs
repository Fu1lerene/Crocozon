using Crocozon.Library.Domain.Abstractions;

namespace Crocozon.Library.EventStore.Abstractions.Processing;

public interface IAggregateProcessor<TAggregate, TId>
    where TAggregate : IAggregate<TId>
    where TId : IAggregateId
{
    public IAggregateProcessingBuilder<TAggregate, TId, TRequest> For<TRequest>(
        IEnumerable<TRequest> requests, Func<TRequest, TId> idSelector);
}