using Crocozon.Library.Domain.Abstractions;
using Crocozon.Library.EventStore.Abstractions;
using Crocozon.Library.EventStore.Abstractions.Processing;
using Crocozon.Library.Exceptions;

namespace Crocozon.Library.EventStore.Processing;

public class AggregateProcessingBuilder<TAggregate, TId, TRequest>(
    IRepository<TAggregate, TId> repository,
    IEnumerable<TRequest> requests,
    Func<TRequest, TId> idSelector) 
    : IAggregateProcessingBuilder<TAggregate, TId, TRequest>
    where TAggregate : IAggregate<TId>
    where TId : IAggregateId
{
    private Func<TId, TRequest, TAggregate>? _missingFunc;
    private Action<TAggregate, TRequest>? _existingAction;
    private Action<TId, TRequest>? _missingAction;

    public IAggregateProcessingBuilder<TAggregate, TId, TRequest> IfNotExists(Func<TId, TRequest, TAggregate> func)
    {
        _missingFunc = func;
        return this;
    }

    public IAggregateProcessingBuilder<TAggregate, TId, TRequest> IfExists(Action<TAggregate, TRequest> action)
    {
        _existingAction = action;
        return this;
    }
    
    public IAggregateProcessingBuilder<TAggregate, TId, TRequest> IfNotExistsAction(Action<TId, TRequest> action)
    {
        _missingAction = action;
        return this;
    }

    public IAggregateProcessingBuilder<TAggregate, TId, TRequest> ThrowIfNotExists()
    {
        _missingAction = (id, _) => throw new AggregateNotFoundException(
            ExceptionMessages.AggregateNotFound(typeof(TAggregate).Name, id.Value));

        return this;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var requestById = requests.ToDictionary(idSelector);
        var aggregatesToSave = new List<TAggregate>(requestById.Count);
        
        await foreach (var (id, aggregate) in repository.GetAsync(requestById.Keys, cancellationToken))
        {
            var request = requestById[id];
            
            if (aggregate is null)
            {
                if (ExecuteMissingFunc(aggregatesToSave, id, request))
                    continue;

                ExecuteMissingAction(id, request);
            }
            else
            {
                if (ExecuteExistingAction(aggregate, request))
                    continue;
                
                aggregatesToSave.Add(aggregate);
            }
        }

        if (aggregatesToSave.Count > 0)
            await repository.SaveAsync(aggregatesToSave, cancellationToken);
    }

    private bool ExecuteMissingFunc(List<TAggregate> aggregatesToSave, TId id, TRequest request)
    {
        if (_missingFunc is null)
            return true;

        aggregatesToSave.Add(_missingFunc(id, request));
        return false;
    }
    
    private void ExecuteMissingAction(TId id, TRequest request) => _missingAction?.Invoke(id, request);
    
    private bool ExecuteExistingAction(TAggregate aggregate, TRequest request)
    {
        if (_existingAction is null)
            return true;

        _existingAction(aggregate, request);
        return false;
    }
}