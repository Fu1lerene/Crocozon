namespace Crocozon.Library.Metadata;

public sealed class EventMetadata
{
    private readonly List<MetadataHeader> _headers = new();

    public IReadOnlyCollection<MetadataHeader> Headers => _headers.AsReadOnly();

    public void Add(string key, byte[] value) => _headers.Add(new MetadataHeader(key, value));
}