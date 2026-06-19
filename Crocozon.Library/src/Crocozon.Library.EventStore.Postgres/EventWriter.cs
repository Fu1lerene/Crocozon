using Crocozon.Library.EventStore.Abstractions;
using Npgsql;

namespace Crocozon.Library.EventStore.Postgres;

public class EventWriter(NpgsqlDataSource dataSource) : IEventWriter
{
    public async Task WriteAsync(Guid aggregateId, long expectedVersion, IReadOnlyCollection<EventData> events, CancellationToken cancellationToken)
    {
        var version = expectedVersion - events.Count;

        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);

        try
        {
            await WriteEventsAsync(connection, aggregateId, version, events, cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private static async Task WriteEventsAsync(NpgsqlConnection connection, Guid aggregateId, long version, IReadOnlyCollection<EventData> events,
        CancellationToken cancellationToken)
    {
        var copyFromCommand = @"
copy domain.events (aggregate_id, version, event_type, payload, metadata) from stdin (format binary)";
        await using var binaryWriter = await connection.BeginBinaryImportAsync(copyFromCommand, cancellationToken);
        
        foreach (var @event in events)
        {
            await binaryWriter.StartRowAsync(cancellationToken);
            await binaryWriter.WriteAsync(aggregateId, cancellationToken);
            await binaryWriter.WriteAsync(++version, cancellationToken);
            await binaryWriter.WriteAsync(@event.Type, cancellationToken);
            await binaryWriter.WriteAsync(@event.Data, cancellationToken);
            await binaryWriter.WriteAsync(@event.Metadata, cancellationToken);
        }
        await binaryWriter.CompleteAsync(cancellationToken);
    }
}