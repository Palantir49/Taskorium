namespace NotificationService.Domain.Enums;

public enum ChannelStatus
{
    Pending = 0, // Ожидает отправки
    Sent = 1, // Успешно отправлено
    Failed = 2, // Ошибка отправки
    Skipped = 3, // Пропущено (нет адреса)
    Retrying = 4 // В процессе повторной отправки
}
