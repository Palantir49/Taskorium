using Grpc.Core;
using Grpc.Core.Interceptors;

namespace FileStorageService.Api.Interceptors;

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
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation error");
            throw new RpcException(new Status(StatusCode.InvalidArgument, ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled server error");
            throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
        }
    }
}
