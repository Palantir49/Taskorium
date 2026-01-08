using System;
using Microsoft.Extensions.DependencyInjection;

namespace TaskService.Application.Mediator;

public class Dispatcher(IServiceProvider serviceProvider) : IDispatcher
{


    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        var handler = serviceProvider.GetRequiredService(handlerType);

        return await (Task<TResponse>)handler.GetType()
            .GetMethod("Handle")!
            .Invoke(handler, [request, cancellationToken])!;
    }

}
