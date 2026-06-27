using Crocozon.Library.EventStore.Abstractions;
using Crocozon.Library.Exceptions;
using Npgsql;

namespace Crocozon.Library.EventStore.Postgres;

public class EventWriter(NpgsqlDataSource dataSource) : IEventWriter
{
    private const string CopyFromCommand = 
        "copy domain.events (aggregate_id, version, event_type, payload, metadata) from stdin (format binary)";
    
    public async Task WriteAsync(EventsDataWriteRequest request, CancellationToken cancellationToken)
    {
        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);

        await TryWriteAsync(() => BinaryWriteEventsAsync(request, connection, cancellationToken),
            transaction, cancellationToken);
    }
    
    public async Task WriteAsync(IReadOnlyCollection<EventsDataWriteRequest> requests, CancellationToken cancellationToken)
    {
        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);

        await TryWriteAsync(() => BinaryWriteEventsAsync(requests, connection, cancellationToken),
            transaction, cancellationToken);
    }
    
    private static async Task TryWriteAsync(Func<Task> write, NpgsqlTransaction transaction, CancellationToken cancellationToken)
    {
        try
        {
            await write();
            await transaction.CommitAsync(cancellationToken);
        }
        catch (PostgresException ex) when (ex.SqlState == PostgresErrorCodes.UniqueViolation)
        {
            throw new AggregateConcurrencyException(ExceptionMessages.AggregateConcurrencyConflict, ex, ex.Detail);
        }
    }

    private static async Task BinaryWriteEventsAsync(EventsDataWriteRequest request, NpgsqlConnection connection,
        CancellationToken cancellationToken)
    {
        var events = request.Events;
        var version = request.ExpectedVersion - events.Count;

        await using var binaryWriter = await connection.BeginBinaryImportAsync(CopyFromCommand, cancellationToken);
        
        await WriteAsync(request.AggregateId, version, events, binaryWriter, cancellationToken);
        await binaryWriter.CompleteAsync(cancellationToken);
    }
    
    private static async Task BinaryWriteEventsAsync(IReadOnlyCollection<EventsDataWriteRequest> requests, NpgsqlConnection connection,
        CancellationToken cancellationToken)
    {
        await using var binaryWriter = await connection.BeginBinaryImportAsync(CopyFromCommand, cancellationToken);

        foreach (var (aggregateId, expectedVersion, events) in requests)
        {
            var version = expectedVersion - events.Count;

            await WriteAsync(aggregateId, version, events, binaryWriter, cancellationToken);
        }
        await binaryWriter.CompleteAsync(cancellationToken);
    }

    private static async Task WriteAsync(Guid aggregateId, long version, IReadOnlyCollection<EventData> events,
        NpgsqlBinaryImporter binaryWriter, CancellationToken cancellationToken)
    {
        foreach (var @event in events)
        {
            await binaryWriter.StartRowAsync(cancellationToken);
            await binaryWriter.WriteAsync(aggregateId, cancellationToken);
            await binaryWriter.WriteAsync(++version, cancellationToken);
            await binaryWriter.WriteAsync(@event.Type, cancellationToken);
            await binaryWriter.WriteAsync(@event.Data, cancellationToken);
            await binaryWriter.WriteAsync(@event.Metadata, cancellationToken);
        }
    }
}