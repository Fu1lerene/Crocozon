using Crocozon.Library.Interceptors;
using Grpc.AspNetCore.Server;

namespace Crocozon.Library.EventStore.Host;

public static class GrpcRegistrator
{
    public static void AddCommonInterceptors(this GrpcServiceOptions options)
    {
        options.Interceptors.Add<ExceptionInterceptor>();
        options.Interceptors.Add<RequestContextInterceptor>();
    }
}