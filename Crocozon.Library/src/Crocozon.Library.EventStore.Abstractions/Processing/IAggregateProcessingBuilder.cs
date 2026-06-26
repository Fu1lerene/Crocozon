using Crocozon.Library.Domain.Abstractions;

namespace Crocozon.Library.EventStore.Abstractions.Processing;

public interface IAggregateProcessingBuilder<TAggregate, out TId, out TRequest>
    where TAggregate : IAggregate<TId>
    where TId : IAggregateId
{
    IAggregateProcessingBuilder<TAggregate, TId, TRequest> IfNotExists(Func<TId, TRequest, TAggregate> func);
    IAggregateProcessingBuilder<TAggregate, TId, TRequest> IfExists(Action<TAggregate, TRequest> action);
    IAggregateProcessingBuilder<TAggregate, TId, TRequest> IfNotExistsAction(Action<TId, TRequest> action);
    IAggregateProcessingBuilder<TAggregate, TId, TRequest> ThrowIfNotExists();
    Task ExecuteAsync(CancellationToken cancellationToken);
}