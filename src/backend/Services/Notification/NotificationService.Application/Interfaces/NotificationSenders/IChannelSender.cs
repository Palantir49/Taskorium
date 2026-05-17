using NotificationService.Domain.Aggregates.Notification;
using NotificationService.Domain.Enums;
using NotificationService.Domain.ValueObjects;
using Taskorium.ServiceDefaults.Result;

namespace NotificationService.Application.Interfaces.NotificationSenders;

public interface IChannelSender
{
    ChannelType ChannelType { get; }

    Task<Result<bool>> SendAsync(NotificationChannel channel, RecipientNotification recipientNotification,
        CancellationToken cancellationToken = default);
}
