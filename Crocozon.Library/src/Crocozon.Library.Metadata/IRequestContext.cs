namespace Crocozon.Library.Metadata;

public interface IRequestContext
{
    string Actor { get; }
    string CorrelationId { get; }
}
