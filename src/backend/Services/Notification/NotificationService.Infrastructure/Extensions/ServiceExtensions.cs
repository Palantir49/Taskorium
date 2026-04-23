using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Infrastructure.Extensions.MessageBus;

namespace NotificationService.Infrastructure.Extensions;

public static class ServiceExtensions
{
    extension(IServiceCollection services)
    {
        public void ConfigureInfrastructureLayer(IConfiguration configuration)
        {
            services.AddMessageBus(configuration);
        }
    }
}
