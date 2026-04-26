using Taskorium.IntegrationEvents.Dto;
using Taskorium.MessageBus.Abstractions;

namespace Taskorium.IntegrationEvents.Notifications;

public record NotificationCreatedIntegrationEvent(NotificationEventContent Content, List<NotificationRecipient> Recipients)
    : IntegrationEvent;
