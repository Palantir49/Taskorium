namespace NotificationService.Domain.Enums;

public enum RecipientNotificationStatus
{
    Pending = 0,
    Delivered = 1,
    PartiallyDelivered = 2,
    Failed = 3,
    Muted = 4 // Получатель заглушен
}
