using System.Text;

namespace Crocozon.Library.Metadata;

public sealed class EventMetadataProvider(IRequestContext requestContext, string source) : IEventMetadataProvider
{
    public EventMetadata Create()
    {
        var metadata = new EventMetadata();

        metadata.Add(MetadataKeys.EventId, Guid.NewGuid().ToByteArray());
        metadata.Add(MetadataKeys.OccurredAt, Encoding.UTF8.GetBytes(DateTimeOffset.UtcNow.ToString("O")));
        metadata.Add(MetadataKeys.CorrelationId, Encoding.UTF8.GetBytes(requestContext.CorrelationId));
        metadata.Add(MetadataKeys.Actor, Encoding.UTF8.GetBytes(requestContext.Actor));
        metadata.Add(MetadataKeys.Source, Encoding.UTF8.GetBytes(source));

        return metadata;
    }
}
