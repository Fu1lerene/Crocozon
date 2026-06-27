namespace Crocozon.Library.Options;

public class LoggingBehaviorOptions
{
    public HashSet<string> SuppressedRequests { get; init; } = [];
    public long SlowRequestThresholdMs { get; init; } = 3000;
}