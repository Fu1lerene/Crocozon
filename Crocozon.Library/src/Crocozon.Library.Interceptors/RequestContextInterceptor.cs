using Crocozon.Library.Metadata;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.DependencyInjection;

namespace Crocozon.Library.Interceptors;

public sealed class RequestContextInterceptor : Interceptor
{
    private const string CorrelationIdHeader = "x-correlation-id";

    public override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        var requestContext = context.GetHttpContext().RequestServices.GetRequiredService<RequestContext>();

        var correlationId = context.RequestHeaders.GetValue(CorrelationIdHeader) ?? Guid.NewGuid().ToString();
        
        requestContext.Initialize(correlationId);

        return continuation(request, context);
    }
}
