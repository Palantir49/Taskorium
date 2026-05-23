using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TaskService.Infrastructure.Outbox.Interfaces;
using TaskService.Infrastructure.Outbox.Options;

namespace TaskService.Infrastructure.Outbox.Processing;

/// <summary>
/// Обёртка для периодического запуска <see cref="IOutboxProcessor"/>.
/// </summary>
public class OutboxProcessorHostedService(
    IOutboxProcessor outboxProcessor,
    IOptions<OutboxOptions> options,
    ILogger<OutboxProcessorHostedService> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await outboxProcessor.ProcessBatchAsync(stoppingToken);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Ошибка в каркасном OutboxProcessorHostedService");
            }

            await Task.Delay(TimeSpan.FromSeconds(options.Value.PollingIntervalSeconds), stoppingToken);
        }
    }
}
