using NotificationService.Domain.Enums;
using NotificationService.Domain.Exceptions;
using NotificationService.Domain.ValueObjects;

namespace NotificationService.Domain.Aggregates.Notification;

public class RecipientNotification
{
    private RecipientNotification(
        Recipient recipient,
        NotificationContent content,
        RecipientNotificationStatus status)
    {
        Recipient = recipient;
        Content = content;
        Status = status;
    }

    public Recipient Recipient { get; private set; }
    public NotificationContent Content { get; private set; }
    public RecipientNotificationStatus Status { get; private set; }
    public List<NotificationChannel> Channels { get; private set; } = [];

    public static RecipientNotification Create(
        Guid notificationId,
        Recipient recipient,
        NotificationContent content)
    {
        var notification = new RecipientNotification(
            recipient, content, RecipientNotificationStatus.Pending);

        notification.InitializeChannels();

        return notification;
    }

    public static RecipientNotification CreateMuted(
        Guid notificationId,
        Recipient recipient,
        NotificationContent content)
    {
        return new RecipientNotification(
            recipient, content, RecipientNotificationStatus.Muted);
    }

    private void InitializeChannels()
    {
        // Определяем доступные каналы для получателя
        var availableChannels = new List<(ChannelType Type, string? Address)>
        {
            (ChannelType.Email, Recipient.Email), (ChannelType.Sms, Recipient.Phone)
        };

        foreach (var (type, address) in availableChannels)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                // Канал недоступен - пропускаем
                Channels.Add(NotificationChannel.Create(type, "unavailable"));
                continue;
            }

            // Если есть предпочтения, проверяем их
            if (Recipient.PreferredChannels.Any() &&
                !Recipient.PreferredChannels.Contains(type))
            {
                // Канал не в списке предпочтительных
                Channels.Add(NotificationChannel.Create(type, address)
                    .MarkAsSkipped("Not in preferred channels"));
                continue;
            }

            // Создаем активный канал для отправки
            Channels.Add(NotificationChannel.Create(type, address));
        }

        // Если все каналы пропущены или недоступны
        if (Channels.All(c => c.Status is ChannelStatus.Skipped or ChannelStatus.Failed))
        {
            Status = RecipientNotificationStatus.Failed;
        }
    }

    public void MarkChannelSent(ChannelType channelType, DateTime sentAt)
    {
        var channel = FindChannel(channelType);
        var index = Channels.IndexOf(channel);
        Channels[index] = channel.MarkAsSent(sentAt);

        UpdateStatus();
    }

    public void MarkChannelFailed(ChannelType channelType, string error)
    {
        var channel = FindChannel(channelType);
        var index = Channels.IndexOf(channel);

        if (channel.CanRetry)
        {
            Channels[index] = channel.RecordRetryAttempt();
        }
        else
        {
            Channels[index] = channel.MarkAsFailed(error);
        }

        UpdateStatus();
    }

    public void SkipChannel(ChannelType channelType, string reason)
    {
        var channel = FindChannel(channelType);
        var index = Channels.IndexOf(channel);
        Channels[index] = channel.MarkAsSkipped(reason);

        UpdateStatus();
    }

    private NotificationChannel FindChannel(ChannelType channelType)
    {
        var channel = Channels.FirstOrDefault(c => c.Type == channelType);
        if (channel == null)
        {
            throw new NotificationDomainException(
                $"Channel {channelType} not found for recipient {Recipient.Id}");
        }

        return channel;
    }

    private void UpdateStatus()
    {
        if (Channels.Count == 0)
        {
            Status = RecipientNotificationStatus.Failed;
            return;
        }

        if (Channels.All(c => c.Status == ChannelStatus.Sent))
        {
            Status = RecipientNotificationStatus.Delivered;
        }
        else if (Channels.All(c =>
                     c.Status is ChannelStatus.Failed or ChannelStatus.Skipped))
        {
            Status = RecipientNotificationStatus.Failed;
        }
        else if (Channels.Any(c => c.Status == ChannelStatus.Sent))
        {
            Status = RecipientNotificationStatus.PartiallyDelivered;
        }
        else
        {
            Status = RecipientNotificationStatus.Pending;
        }
    }
}
