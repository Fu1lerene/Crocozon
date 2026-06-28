namespace Crocozon.Library.Metadata;

public sealed class EventMetadata
{
    private readonly List<MetadataHeader> _headers = [];

    public IReadOnlyCollection<MetadataHeader> Headers => _headers.AsReadOnly();

    public void Add(string key, byte[] value) => _headers.Add(new MetadataHeader(key, value));

    public byte[]? GetValueOrDefault(string key)
        => _headers.FirstOrDefault(header => header.Key == key)?.Value;
}