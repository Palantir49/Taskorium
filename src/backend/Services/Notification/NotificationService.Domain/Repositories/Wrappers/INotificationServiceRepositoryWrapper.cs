using NotificationService.Domain.Repositories.Interfaces.Notifications;
using NotificationService.Domain.Repositories.Interfaces.OutBox;

namespace NotificationService.Domain.Repositories.Wrappers;

public interface INotificationServiceRepositoryWrapper
{
    INotificationRepository NotificationRepository { get; }

    IOutBoxRepository OutBoxRepository { get; }

    Task BeginTransactionAsync(CancellationToken ct = default);
    Task CommitAsync(CancellationToken ct = default);
    Task RollbackAsync(CancellationToken ct = default);
}
