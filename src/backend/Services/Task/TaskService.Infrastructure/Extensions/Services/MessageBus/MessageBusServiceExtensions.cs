using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Taskorium.MessageBus.RabbitMq;

namespace TaskService.Infrastructure.Extensions.Services.MessageBus
{
    public static class ServiceExtensions
    {
        extension(IServiceCollection services)
        {
            internal void AddMessageBus(IConfiguration configuration)
            {
                services.AddRabbitMqPublisher(configuration);
            }
        }
    }
}
