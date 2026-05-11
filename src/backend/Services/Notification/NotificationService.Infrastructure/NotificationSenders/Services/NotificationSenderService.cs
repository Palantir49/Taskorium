using Microsoft.Extensions.Logging;
using NotificationService.Application.Interfaces.NotificationSenders;
using NotificationService.Domain.Aggregates.Notification;
using NotificationService.Domain.Enums;
using NotificationService.Domain.ValueObjects;

namespace NotificationService.Infrastructure.NotificationSenders.Services;

public sealed class NotificationSender(
    IEnumerable<IChannelSender> channelSenders,
    ILogger<NotificationSender> logger
) : INotificationSender
{
    private readonly Dictionary<ChannelType, IChannelSender> _senders =
        channelSenders.ToDictionary(s => s.ChannelType);

    public async Task SendAsync(
        Notification notification,
        CancellationToken ct = default)
    {
        var noneMutedRecipients =
            notification.RecipientNotifications.Where(element => element.Status != RecipientNotificationStatus.Muted)
                .ToList();

        foreach (var rn in noneMutedRecipients)
        {
            await SendToRecipientAsync(notification, rn, ct);
        }
    }

    private async Task SendToRecipientAsync(
        Notification notification,
        RecipientNotification rn,
        CancellationToken ct)
    {
        var pendingChannels = rn.Channels.Where(element => element.Status == ChannelStatus.Pending).ToList();
        foreach (var channel in pendingChannels)
        {
            await SendChannelAsync(notification, rn, channel, ct);
        }
    }

    private async Task SendChannelAsync(
        Notification notification,
        RecipientNotification rn,
        NotificationChannel channel,
        CancellationToken ct)
    {
        if (!_senders.TryGetValue(channel.Type, out var sender))
        {
            logger.LogWarning(
                "Незарегистрировано ни одного обработчик для канала типа:  {Type}. Для получателя {RecipientId}",
                channel.Type, rn.Recipient.Id);

            notification.SkipRecipientChannel(
                rn.Recipient.Id,
                channel.Type,
                $"Незарегистрировано ни одного обработчик для канала типа {channel.Type}");
            return;
        }

        logger.LogInformation(
            "Отправка сообщения по каналу {ChannelType} по {Address} для получателя {RecipientId}",
            channel.Type, channel.Address, rn.Recipient.Id);

        var result = await sender.SendAsync(channel, rn, ct);

        if (result.Value)
        {
            notification.MarkRecipientChannelSent(
                rn.Recipient.Id,
                channel.Type,
                DateTime.UtcNow);

            logger.LogInformation(
                "Сообщение успешно отправлено по каналу {ChannelType} по адресу {Address}",
                channel.Type, channel.Address);
        }
        else
        {
            notification.MarkRecipientChannelFailed(
                rn.Recipient.Id,
                channel.Type,
                result.Error ?? "Неизвестная ошибка");

            logger.LogWarning(
                "Во время отправки ссобщения по каналу типа {ChannelType} по адресу {Address} произошла ошибка: {Error}",
                channel.Type, channel.Address, result.Error);
        }
    }
}
