namespace NotificationService.Domain.Enums;

public enum NotificationStatus
{
    Pending = 0, // Создано, ожидает обработки
    Processing = 1, // В процессе отправки
    Delivered = 2, // Доставлено всем получателям
    PartiallyDelivered = 3, // Доставлено части получателей
    Failed = 4, // Не доставлено никому
    Cancelled = 5 // Отменено
}
