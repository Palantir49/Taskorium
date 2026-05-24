using Microsoft.Extensions.Logging;
using Taskorium.IntegrationEvents.Notifications;
using Taskorium.MessageBus.Abstractions;
using TaskService.Infrastructure.Outbox.Interfaces;
using TaskService.Infrastructure.Outbox.Models;

namespace TaskService.Infrastructure.Outbox.Processing;

/// <summary>
/// Публикует outbox-сообщение в message bus.
/// Отвечает за преобразование payload в конкретный integration event.
/// </summary>
public class OutboxPublisher(
    ILogger<OutboxPublisher> logger,
    IOutboxSerializer outboxSerializer) : IOutboxPublisher
{
    public async Task PublishAsync(OutboxMessage message, IEventBus eventBus, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(message);
        ArgumentNullException.ThrowIfNull(eventBus);

        if (string.IsNullOrWhiteSpace(message.Type) || string.IsNullOrWhiteSpace(message.Payload))
            throw new ArgumentException("У outbox-сообщения отсутствует Type или Payload");

        switch (message.Type)
        {
            case nameof(NotificationCreatedIntegrationEvent):
            {
                var integrationEvent = outboxSerializer.Deserialize<NotificationCreatedIntegrationEvent>(message.Payload);
                await eventBus.PublishAsync(integrationEvent, cancellationToken);
                break;
            }
            default:
                logger.LogWarning("Неизвестный тип outbox-сообщения: {MessageType}, id: {OutboxMessageId}", message.Type, message.Id);
                throw new InvalidOperationException($"Неизвестный тип outbox-сообщения: {message.Type}");
        }
    }
}
