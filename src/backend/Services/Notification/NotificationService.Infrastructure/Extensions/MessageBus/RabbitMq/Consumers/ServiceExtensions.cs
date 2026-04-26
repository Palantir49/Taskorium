using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Integration.EventHandlers;
using Taskorium.IntegrationEvents.Notifications;
using Taskorium.MessageBus.RabbitMq;

namespace NotificationService.Infrastructure.Extensions.MessageBus.RabbitMq.Consumers;

internal static class ServiceExtensions
{
    extension(IServiceCollection serviceCollection)
    {
        internal void AddRabbitMqConsumers(IConfiguration configuration)
        {
            serviceCollection.AddRabbitMqConsumer<NotificationIntegrationEvent, IssueCreatedEventHandler>(configuration,
                "notification-received");
        }
    }
}
