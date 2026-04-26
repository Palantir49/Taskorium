using Taskorium.MessageBus.RabbitMq.Subscriptions;

namespace Taskorium.MessageBus.RabbitMq.Options;

public interface IRabbitMqSubscriptionDescriptor
{
    ISubscriptionDefinition Create();
}
