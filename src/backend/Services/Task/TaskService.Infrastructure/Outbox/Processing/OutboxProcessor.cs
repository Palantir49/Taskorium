using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Taskorium.MessageBus.Abstractions;
using TaskService.Infrastructure.Outbox.Interfaces;
using TaskService.Infrastructure.Outbox.Models;
using TaskService.Infrastructure.Outbox.Options;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Infrastructure.Outbox.Processing;

/// <summary>
/// Логика выборки pending-сообщений, публикации в шину и обновления статусов.
/// </summary>
public class OutboxProcessor(
    ILogger<OutboxProcessor> logger,
    IOptions<OutboxOptions> options,
    TaskServiceDbContext context,
    IEventBus eventBus,
    IOutboxPublisher outboxPublisher)
    : IOutboxProcessor
{
    /// <summary>
    /// Обработать одну порцию outbox-сообщений.
    /// </summary>
    public async Task ProcessBatchAsync(CancellationToken cancellationToken = default)
    {
        var messages = await context.OutboxMessages
            .Where(x => (x.Status == OutboxStatuses.Pending || x.Status == OutboxStatuses.Failed)
                        && x.Retries < options.Value.MaxRetries)
            .OrderBy(x => x.OccurredOnUtc)
            .Take(options.Value.BatchSize)
            .ToListAsync(cancellationToken);

        if (messages.Count == 0)
            return;

        foreach (var message in messages)
        {
            try
            {
                await outboxPublisher.PublishAsync(message, eventBus, cancellationToken);

                message.MarkProcessed();
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Ошибка обработки outbox-сообщения {OutboxMessageId}", message.Id);
                message.MarkFailed(exception.Message);
            }
        }

        await context.SaveChangesAsync(cancellationToken);
    }
}
