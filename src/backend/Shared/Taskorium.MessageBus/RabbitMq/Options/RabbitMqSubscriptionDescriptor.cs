using Taskorium.MessageBus.Abstractions;
using Taskorium.MessageBus.RabbitMq.Subscriptions;

namespace Taskorium.MessageBus.RabbitMq.Options;

public sealed class RabbitMqSubscriptionDescriptor<TEvent, THandler>(RabbitMqSubscriptionOptions options)
    : IRabbitMqSubscriptionDescriptor
    where TEvent : IntegrationEvent
    where THandler : class, IEventHandler<TEvent>
{
    public ISubscriptionDefinition Create()
    {
        return new SubscriptionDefinition<TEvent, THandler>(options);
    }
}
