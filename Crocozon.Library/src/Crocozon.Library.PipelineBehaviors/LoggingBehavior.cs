using System.Diagnostics;
using Crocozon.Library.Options;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Crocozon.Library.PipelineBehaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger, IOptions<LoggingBehaviorOptions> options)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly LoggingBehaviorOptions _options = options.Value;
    
    private const string LogHandleTime = "Handled {Request} in {Ms}ms.";
    
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        
        if (_options.SuppressedRequests.Contains(requestName))
            return await next(cancellationToken);

        var stopWatch = Stopwatch.StartNew();
        var response = await next(cancellationToken);
        stopWatch.Stop();
        
        var elapsedMs = stopWatch.ElapsedMilliseconds;
        
        if (elapsedMs >= _options.SlowRequestThresholdMs)
            logger.LogWarning(LogHandleTime, requestName, elapsedMs);
        else if (logger.IsEnabled(LogLevel.Debug))
            logger.LogDebug(LogHandleTime, requestName, elapsedMs);

        return response;
    }
}