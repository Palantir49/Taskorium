using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Domain.Repositories.Wrappers;
using NotificationService.Infrastructure.Extensions.Persistence.Mongo;
using NotificationService.Infrastructure.Persistence.Mongo.Repositories.Wrappers;

namespace NotificationService.Infrastructure.Extensions.Persistence;

internal static class ServiceExtensions
{
    extension(IServiceCollection serviceCollection)
    {
        internal void AddPersistence(IConfiguration configuration)
        {
            serviceCollection.AddMongo(configuration);
            serviceCollection.AddScoped<INotificationServiceRepositoryWrapper, NotificationServiceRepositoryWrapper>();
        }
    }
}
