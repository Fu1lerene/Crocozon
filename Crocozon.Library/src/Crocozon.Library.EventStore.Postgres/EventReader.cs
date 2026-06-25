using System.Runtime.CompilerServices;
using Crocozon.Library.EventStore.Abstractions;
using Crocozon.Library.Extensions.Postgres;
using Npgsql;

namespace Crocozon.Library.EventStore.Postgres;

public class EventReader(NpgsqlDataSource dataSource) : IEventReader
{
    public async Task<IReadOnlyCollection<RecordedEvent>> ReadAsync(Guid aggregateId, CancellationToken cancellationToken)
    {
        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);

        var command = connection.CreateCommand();
        command.CommandText = @"
select version, event_type, payload, metadata, created
from domain.events
where aggregate_id = $1
order by version;";

        command.AddParameter(aggregateId);

        return await ReadEventsAsync(aggregateId, command, cancellationToken);
    }
    
    public async IAsyncEnumerable<RecordedEvent> ReadAsync(IReadOnlyCollection<Guid> aggregateIds, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);

        var command = connection.CreateCommand();
        command.CommandText = @"
select aggregate_id, version, event_type, payload, metadata, created
from domain.events
where aggregate_id = any($1)
order by aggregate_id, version;";

        command.AddParameter(aggregateIds.ToArray());

        await foreach (var recordedEvent in ReadEventsAsync(command, cancellationToken)) 
            yield return recordedEvent;
    }

    private static async Task<List<RecordedEvent>> ReadEventsAsync(Guid aggregateId, NpgsqlCommand command, CancellationToken cancellationToken)
    {
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        var events = new List<RecordedEvent>();
        while (await reader.ReadAsync(cancellationToken))
        {
            events.Add(new RecordedEvent(
                aggregateId,
                reader.GetInt64(0),
                reader.GetString(1),
                await reader.GetFieldValueAsync<byte[]>(2, cancellationToken),
                await reader.GetFieldValueAsync<byte[]>(3, cancellationToken),
                reader.GetDateTime(4)));
        }
        return events;
    }
    
    private static async IAsyncEnumerable<RecordedEvent> ReadEventsAsync(NpgsqlCommand command, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            yield return new RecordedEvent(
                reader.GetGuid(0),
                reader.GetInt64(1),
                reader.GetString(2),
                await reader.GetFieldValueAsync<byte[]>(3, cancellationToken),
                await reader.GetFieldValueAsync<byte[]>(4, cancellationToken),
                reader.GetDateTime(5));
        }
    }
}