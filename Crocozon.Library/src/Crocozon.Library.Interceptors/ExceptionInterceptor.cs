using Crocozon.Library.Exceptions;
using Google.Rpc;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;
using Status = Grpc.Core.Status;
using ValidationException = FluentValidation.ValidationException;

namespace Crocozon.Library.Interceptors;

public class ExceptionInterceptor(ILogger<ExceptionInterceptor> logger) : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (RpcException)
        {
            throw;
        }
        catch (ValidationException ex)
        {
            var violations = ex.Errors.Select(validationError => new BadRequest.Types.FieldViolation
            {
                Field = validationError.PropertyName,
                Description = validationError.ErrorMessage
            });
        
            var badRequest = new BadRequest
            {
                FieldViolations = { violations }
            };
        
            var status = new Google.Rpc.Status
            {
                Code = (int)StatusCode.InvalidArgument,
                Message = "Validation failed.",
                Details = { Google.Protobuf.WellKnownTypes.Any.Pack(badRequest) }
            };
        
            throw status.ToRpcException();
        }
        catch (AggregateNotFoundException ex)
        {
            throw new RpcException(new Status(StatusCode.NotFound, ex.Message));
        }
        catch (AggregateConcurrencyException ex)
        {
            var errorInfo = new ErrorInfo
            {
                Reason = "AGGREGATE_CONCURRENCY_CONFLICT",
                Domain = context.Host
            };
            var status = new Google.Rpc.Status
            {
                Code = (int)StatusCode.Aborted,
                Message = ex.Message,
                Details = { Google.Protobuf.WellKnownTypes.Any.Pack(errorInfo) }
            };
            logger.LogError(ex, "Concurrency conflict, detail: {Detail}.", ex.Detail);
            
            throw status.ToRpcException();
        }
        catch (OperationCanceledException ex) when (context.CancellationToken.IsCancellationRequested)
        {
            throw new RpcException(new Status(StatusCode.Cancelled, ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception in {Method}.", context.Method);
            throw new RpcException(new Status(StatusCode.Internal, "Internal server error."));
        }
    }
}