using Crocozon.Library.EventStore.Proto;
using Crocozon.Library.Metadata;
using Google.Protobuf;

namespace Crocozon.Library.EventStore.Protobuf.Extensions;

public static class MetadataProtoExtensions
{
    public static EventMetadataProto ToProto(this EventMetadata metadata)
        => new()
        {
            MetadataHeaders =
            {
                metadata.Headers.Select(header => new MetadataHeaderProto
                {
                    Key = header.Key,
                    Value = ByteString.CopyFrom(header.Value)
                })
            }
        };

    public static EventMetadata ToModel(this EventMetadataProto proto)
    {
        var metadata = new EventMetadata();
        foreach (var header in proto.MetadataHeaders)
        {
            metadata.Add(header.Key, header.Value.ToByteArray());
        }
        return metadata;
    }

    public static EventMetadata Deserialize(byte[] bytes)
    {
        var proto = EventMetadataProto.Parser.ParseFrom(bytes);
        return proto.ToModel();
    }
}