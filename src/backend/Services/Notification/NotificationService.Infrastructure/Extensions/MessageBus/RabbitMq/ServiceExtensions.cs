using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Infrastructure.Extensions.MessageBus.RabbitMq.Consumers;

namespace NotificationService.Infrastructure.Extensions.MessageBus.RabbitMq;

internal static class ServiceExtensions
{
    extension(IServiceCollection serviceCollection)
    {
        internal void AddRabbitMq(IConfiguration configuration)
        {
            serviceCollection.AddRabbitMqConsumers(configuration);
        }
    }
}
