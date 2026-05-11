using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NotificationService.Infrastructure.Persistence.Mongo.Contexts;

namespace NotificationService.Infrastructure.Persistence.Mongo.Indexes;

public sealed class IndexInitializer(ILogger<IndexInitializer> logger, IServiceScopeFactory scopeFactory)
    : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Инициализация индексов БД Notification Service");

        try
        {
            await using var scope = scopeFactory.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<NotificationDbContext>();
            await dbContext.EnsureNotificationIndexesAsync(cancellationToken);
            logger.LogInformation("Индексы проинициализированы");
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "В процессе инициализации индексов произошла ошибка");
            throw;
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}
