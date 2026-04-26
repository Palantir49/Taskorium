using Taskorium.IntegrationEvents.Dto;
using Taskorium.MessageBus.Abstractions;

namespace Taskorium.IntegrationEvents.Notifications;

public record NotificationIntegrationEvent(NotificationEventContent Content, List<NotificationRecipient> Recipients)
    : IntegrationEvent;
