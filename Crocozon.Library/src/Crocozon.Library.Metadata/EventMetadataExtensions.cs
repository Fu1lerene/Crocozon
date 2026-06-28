using System.Globalization;
using System.Text;
using Crocozon.Library.Exceptions;

namespace Crocozon.Library.Metadata;

public static class EventMetadataExtensions
{
    public static Guid GetEventId(this EventMetadata metadata)
        => new(GetRequired(metadata, MetadataKeys.EventId));

    public static DateTimeOffset GetOccurredAt(this EventMetadata metadata)
        => DateTimeOffset.ParseExact(
            GetRequiredString(metadata, MetadataKeys.OccurredAt),
            "O",
            CultureInfo.InvariantCulture,
            DateTimeStyles.RoundtripKind);

    public static string GetCorrelationId(this EventMetadata metadata)
        => GetRequiredString(metadata, MetadataKeys.CorrelationId);

    public static string GetActor(this EventMetadata metadata)
        => GetRequiredString(metadata, MetadataKeys.Actor);

    public static string GetSource(this EventMetadata metadata)
        => GetRequiredString(metadata, MetadataKeys.Source);

    private static string GetRequiredString(EventMetadata metadata, string key)
        => Encoding.UTF8.GetString(GetRequired(metadata, key));

    private static byte[] GetRequired(EventMetadata metadata, string key)
        => metadata.GetValueOrDefault(key)
           ?? throw new InvalidOperationException(ExceptionMessages.MetadataKeyNotFound(key));
}
