namespace Crocozon.Library.Metadata;

public sealed class RequestContext : IRequestContext
{
    public const string SystemActor = "system";

    public string Actor { get; private set; } = SystemActor;
    public string CorrelationId { get; private set; } = Guid.NewGuid().ToString();

    public void Initialize(string correlationId, string? actor = null)
    {
        if (!string.IsNullOrWhiteSpace(correlationId))
            CorrelationId = correlationId;

        if (!string.IsNullOrWhiteSpace(actor))
            Actor = actor;
    }
}
