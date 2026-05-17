using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Infrastructure.Extensions.MessageBus.RabbitMq;

namespace NotificationService.Infrastructure.Extensions.MessageBus;

internal static class ServiceExtensions
{
    extension(IServiceCollection services)
    {
        internal void AddMessageBus(IConfiguration configuration)
        {
            services.AddRabbitMq(configuration);
        }
    }
}
