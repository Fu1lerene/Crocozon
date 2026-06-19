using Crocozon.Library.Exceptions;

namespace Crocozon.Library.Metadata;

public sealed record MetadataHeader
{
    public string Key { get; }
    public byte[] Value { get; }
    
    public MetadataHeader(string key, byte[] value)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException(ExceptionMessages.ValueCannotBeNullOrWhitespace, nameof(key));

        Key = key;
        Value = value;
    }
}