namespace Crocozon.Library.EventStore.Abstractions;

public sealed record EventData(string Type, byte[] Data, byte[] Metadata);