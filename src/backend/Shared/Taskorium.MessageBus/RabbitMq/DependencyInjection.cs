using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Taskorium.MessageBus.Abstractions;
using Taskorium.MessageBus.RabbitMq.Infrastructure;
using Taskorium.MessageBus.RabbitMq.Options;
using Taskorium.MessageBus.RabbitMq.Serialization;
using Taskorium.MessageBus.RabbitMq.Topology;

namespace Taskorium.MessageBus.RabbitMq;

public static class DependencyInjection
{
    private static void AddCommonRabbitMqServices(
        IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddOptions<RabbitMqOptions>()
            .Bind(configuration.GetSection(RabbitMqOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.TryAddSingleton<IEventSerializer, SystemTextJsonEventSerializer>();
        services.TryAddSingleton<IEventTopologyRegistry, RabbitMqTopologyRegistry>();
    }

    private static void ValidateSubscription(
        RabbitMqSubscriptionOptions options,
        string subscriptionName)
    {
        if (string.IsNullOrWhiteSpace(options.Exchange))
        {
            throw new InvalidOperationException(
                $"RabbitMq subscription '{subscriptionName}' has no Exchange.");
        }

        if (string.IsNullOrWhiteSpace(options.Queue))
        {
            throw new InvalidOperationException(
                $"RabbitMq subscription '{subscriptionName}' has no Queue.");
        }

        if (string.IsNullOrWhiteSpace(options.RoutingKey))
        {
            throw new InvalidOperationException(
                $"RabbitMq subscription '{subscriptionName}' has no RoutingKey.");
        }
    }

    extension(IServiceCollection services)
    {
        public IServiceCollection AddRabbitMqPublisher(IConfiguration configuration)
        {
            AddCommonRabbitMqServices(services, configuration);

            services.TryAddSingleton<IEventBus, RabbitMqEventBus>();

            return services;
        }

        public IServiceCollection AddRabbitMqConsumer<TEvent, THandler>(IConfiguration configuration,
            string subscriptionName)
            where TEvent : IntegrationEvent
            where THandler : class, IEventHandler<TEvent>
        {
            AddCommonRabbitMqServices(services, configuration);

            services.TryAddSingleton<IEventBus, RabbitMqEventBus>();
            services.AddHostedService<EventBusHostedService>();

            services.AddScoped<THandler>();

            var options = new RabbitMqSubscriptionOptions();
            configuration
                .GetSection($"{RabbitMqOptions.SectionName}:Subscriptions:{subscriptionName}")
                .Bind(options);

            ValidateSubscription(options, subscriptionName);

            services.AddSingleton<IRabbitMqSubscriptionDescriptor>(
                new RabbitMqSubscriptionDescriptor<TEvent, THandler>(options));

            return services;
        }
    }
}
