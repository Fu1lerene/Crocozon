namespace Crocozon.Library.Exceptions;

public static class ExceptionMessages
{
    public const string ValueCannotBeNegative = "Value cannot be negative.";
    public const string ValueMustBePositive = "Value must be greater than zero.";
    public const string ValueCannotBeNullOrWhitespace = "Value cannot be null or whitespace.";
    public const string UnknownCurrencyCode = "Unknown currency code.";
    public const string AggregateConcurrencyConflict = "Aggregate concurrency conflict.";
    public static string ItemNameTooLong(short maxLengthName) => $"The item name is too long, it must be less than {maxLengthName} characters.";
    public static string EventTypeNotFound(string eventType) => $"Event type not found: {eventType}.";
    public static string DescriptorNotFound(string protoTypeFullName) => $"Protobuf message descriptor for type '{protoTypeFullName}' was not found in the provided descriptors list. Ensure that this proto message has been registered for deserializer.";
    public static string DomainEventAlreadyMapped(string domainTypeName) => $"Mapping for domain event type '{domainTypeName}' has already been registered.";
    public static string ProtoMessageAlreadyMapped(string descriptorFullName) => $"Protobuf message with full name '{descriptorFullName}' has already been mapped to a domain event.";
    public static string AggregateNotFound(string aggregateName, Guid id) => $"Aggregate {aggregateName} with id {id} not found.";
    public static string MetadataKeyNotFound(string key) => $"Metadata key '{key}' not found.";
    public static string DuplicateItemIdsRequest(IReadOnlyCollection<long> ids) => $"Request contains duplicate item ids: {string.Join(", ", ids)}.";
    public static string EmptyItemName(IReadOnlyCollection<long> ids) => $"Request contains empty item names for ids: {string.Join(", ", ids)}.";
}