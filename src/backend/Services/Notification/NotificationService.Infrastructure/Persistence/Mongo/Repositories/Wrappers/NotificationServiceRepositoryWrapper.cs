using NotificationService.Domain.Repositories.Interfaces.Notifications;
using NotificationService.Domain.Repositories.Interfaces.OutBox;
using NotificationService.Domain.Repositories.Wrappers;
using NotificationService.Infrastructure.Persistence.Mongo.Contexts;
using NotificationService.Infrastructure.Persistence.Mongo.Repositories.Notification;
using NotificationService.Infrastructure.Persistence.Mongo.Repositories.OutBox;

namespace NotificationService.Infrastructure.Persistence.Mongo.Repositories.Wrappers;

public class NotificationServiceRepositoryWrapper(NotificationDbContext notificationDbContext)
    : INotificationServiceRepositoryWrapper
{
    public INotificationRepository NotificationRepository
    {
        get
        {
            field ??= new NotificationRepository(notificationDbContext);
            return field;
        }
    }

    public IOutBoxRepository OutBoxRepository
    {
        get
        {
            field ??= new OutboxRepository(notificationDbContext);
            return field;
        }
    }

    public async Task BeginTransactionAsync(CancellationToken ct = default)
    {
        notificationDbContext.CurrentSession = await notificationDbContext.StartSessionAsync(ct);
        notificationDbContext.CurrentSession.StartTransaction();
    }

    public async Task CommitAsync(CancellationToken ct = default)
    {
        EnsureSession();
        await notificationDbContext.CurrentSession!.CommitTransactionAsync(ct);
        DisposeSession();
    }

    public async Task RollbackAsync(CancellationToken ct = default)
    {
        if (notificationDbContext.CurrentSession is not null)
        {
            await notificationDbContext.CurrentSession.CommitTransactionAsync(ct);
            DisposeSession();
        }
    }

    private void EnsureSession()
    {
        if (notificationDbContext.CurrentSession is null)
        {
            throw new InvalidOperationException(
                "No active transaction. Call BeginTransactionAsync first.");
        }
    }


    private void DisposeSession()
    {
        notificationDbContext.CurrentSession?.Dispose();
        notificationDbContext.CurrentSession = null;
    }
}
