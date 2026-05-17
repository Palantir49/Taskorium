using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Infrastructure.Outbox.Options;
using NotificationService.Infrastructure.Outbox.Services;

namespace NotificationService.Infrastructure.Extensions.Outbox;

internal static class ServiceExtensions
{
    extension(IServiceCollection serviceCollection)
    {
        internal void ConfigureOutbox(IConfiguration configuration)
        {
            serviceCollection.AddOptions<OutboxOptions>().Bind(configuration.GetSection("OutBox"))
                .ValidateDataAnnotations().ValidateOnStart();

            serviceCollection.AddHostedService<OutboxProcessor>();
        }
    }
}
