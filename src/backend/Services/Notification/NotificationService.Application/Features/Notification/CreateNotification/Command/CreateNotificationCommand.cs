using NotificationService.Application.Abstractions;
using NotificationService.Application.Features.Notification.CreateNotification.Result;
using Taskorium.IntegrationEvents.Dto;

namespace NotificationService.Application.Features.Notification.CreateNotification.Command;

public record CreateNotificationCommand(
    Guid IdempotencyKey,
    DateTimeOffset CreatedAtUtc,
    NotificationEventContent Content,
    List<NotificationRecipient> Recipients
) : ICommand<CreateNotificationResult>;
