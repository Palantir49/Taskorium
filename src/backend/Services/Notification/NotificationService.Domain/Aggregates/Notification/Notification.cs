using NotificationService.Domain.Enums;
using NotificationService.Domain.Exceptions;
using NotificationService.Domain.ValueObjects;

namespace NotificationService.Domain.Aggregates.Notification;

public class Notification
{
    // Получатели
    private readonly List<RecipientNotification> _recipientNotifications = [];

    private Notification(
        IdempotencyKey eventIdempotencyKey,
        IEnumerable<Recipient> recipients,
        NotificationContent content)
    {
        Id = NotificationId.CreateNew();
        EventIdempotencyKey = eventIdempotencyKey;
        Status = NotificationStatus.Pending;
        CreatedAt = DateTime.UtcNow;

        InitializeRecipients(recipients, content);
    }

    public NotificationId Id { get; }
    public IdempotencyKey EventIdempotencyKey { get; private set; }


    public NotificationStatus Status { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset CompletedAt { get; private set; }

    public IReadOnlyCollection<RecipientNotification> RecipientNotifications
        => _recipientNotifications.AsReadOnly();


    // Фабричный метод
    public static Notification Create(
        IdempotencyKey eventIdempotencyKey,
        IEnumerable<Recipient> recipients,
        NotificationContent content)
    {
        var notification = new Notification(
            eventIdempotencyKey, recipients, content);

        // Валидация
        notification.Validate();

        return notification;
    }

    private void InitializeRecipients(
        IEnumerable<Recipient> recipients, NotificationContent content)
    {
        foreach (var recipient in recipients)
        {
            // Пропускаем заглушенных пользователей
            if (recipient.IsMuted)
            {
                _recipientNotifications.Add(
                    RecipientNotification.CreateMuted(Id, recipient, content));
                continue;
            }

            var recipientNotification = RecipientNotification.Create(
                Id, recipient, content);
            _recipientNotifications.Add(recipientNotification);
        }
    }

    private void Validate()
    {
        if (_recipientNotifications.Count == 0)
        {
            throw new NotificationDomainException(
                "Notification must have at least one recipient");
        }
    }

    // Начало обработки
    public void StartProcessing()
    {
        if (Status != NotificationStatus.Pending)
        {
            throw new InvalidOperationException(
                $"Cannot start processing notification in {Status} state");
        }

        Status = NotificationStatus.Processing;
    }

    // Отметить успешную отправку канала для получателя
    public void MarkRecipientChannelSent(
        RecipientId recipientId, ChannelType channel, DateTime sentAt)
    {
        var recipientNotification = FindRecipient(recipientId);

        recipientNotification.MarkChannelSent(channel, sentAt);
        UpdateOverallStatus();
    }

    // Отметить ошибку отправки канала для получателя
    public void MarkRecipientChannelFailed(
        RecipientId recipientId, ChannelType channel, string error)
    {
        var recipientNotification = FindRecipient(recipientId);
        recipientNotification.MarkChannelFailed(channel, error);

        UpdateOverallStatus();
    }

    // Пропустить канал (недоступен)
    public void SkipRecipientChannel(
        RecipientId recipientId, ChannelType channel, string reason)
    {
        var recipientNotification = FindRecipient(recipientId);
        recipientNotification.SkipChannel(channel, reason);

        UpdateOverallStatus();
    }

    private RecipientNotification FindRecipient(RecipientId recipientId)
    {
        var recipient = _recipientNotifications
            .FirstOrDefault(r => r.Recipient.Id == recipientId);

        if (recipient == null)
        {
            throw new NotificationDomainException(
                $"Recipient {recipientId} not found in notification");
        }

        return recipient;
    }

    private void UpdateOverallStatus()
    {
        var allChannels = _recipientNotifications
            .SelectMany(r => r.Channels)
            .ToList();

        if (!allChannels.Any())
        {
            Status = NotificationStatus.Failed;
            CompletedAt = DateTime.UtcNow;
            return;
        }

        var sentCount = allChannels.Count(c => c.Status == ChannelStatus.Sent);
        var failedCount = allChannels.Count(c => c.Status == ChannelStatus.Failed);
        var skippedCount = allChannels.Count(c => c.Status == ChannelStatus.Skipped);
        var totalCount = allChannels.Count;

        if (sentCount == totalCount)
        {
            Status = NotificationStatus.Delivered;
            CompletedAt = DateTime.UtcNow;
        }
        else if (failedCount + skippedCount == totalCount)
        {
            Status = NotificationStatus.Failed;
            CompletedAt = DateTime.UtcNow;
        }
        else if (sentCount > 0)
        {
            Status = NotificationStatus.PartiallyDelivered;
        }
        else
        {
            Status = NotificationStatus.Processing;
        }
    }

    // Получить каналы для повторной отправки
    public IReadOnlyCollection<(RecipientId RecipientId, NotificationChannel Channel)>
        GetRetryableChannels()
    {
        return _recipientNotifications
            .SelectMany(r => r.Channels
                .Where(c => c.CanRetry)
                .Select(c => (r.Recipient.Id, c)))
            .ToList();
    }

    // Проверить, можно ли повторить отправку
    public bool CanRetry()
    {
        return Status is NotificationStatus.Failed or NotificationStatus.PartiallyDelivered;
    }

    // Отмена уведомления
    public void Cancel(string reason)
    {
        if (Status == NotificationStatus.Delivered)
        {
            throw new InvalidOperationException("Cannot cancel delivered notification");
        }

        Status = NotificationStatus.Cancelled;
        CompletedAt = DateTime.UtcNow;
    }
}
