using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Infrastructure.Extensions.MessageBus;
using NotificationService.Infrastructure.Extensions.Persistence;

namespace NotificationService.Infrastructure.Extensions;

public static class ServiceExtensions
{
    extension(IServiceCollection services)
    {
        public void ConfigureInfrastructureLayer(IConfiguration configuration)
        {
            services.AddMessageBus(configuration);
            services.AddPersistence(configuration);
        }
    }
}
