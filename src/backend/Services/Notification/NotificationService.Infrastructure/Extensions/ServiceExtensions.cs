using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Infrastructure.Extensions.MessageBus;
using NotificationService.Infrastructure.Extensions.NotificationSenders;
using NotificationService.Infrastructure.Extensions.Outbox;
using NotificationService.Infrastructure.Extensions.Persistence;

namespace NotificationService.Infrastructure.Extensions;

public static class ServiceExtensions
{
    extension(IServiceCollection services)
    {
        public void ConfigureInfrastructureLayer(IConfiguration configuration)
        {
            services.AddPersistence(configuration);
            services.AddMessageBus(configuration);
            services.ConfigureEmailSenderService(configuration);
            services.ConfigureOutbox(configuration);
        }
    }
}
