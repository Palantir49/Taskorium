using Microsoft.Extensions.Hosting;
using Taskorium.MessageBus.Abstractions;

namespace Taskorium.MessageBus.RabbitMq.Infrastructure;

public sealed class EventBusHostedService(IEventBus eventBus) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        return eventBus.StartAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return eventBus.StopAsync(cancellationToken);
    }
}
