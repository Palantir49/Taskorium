using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Mapping;

namespace NotificationService.Application.Extensions.Mapping;

internal static class ServiceExtensions
{
    extension(IServiceCollection serviceCollection)
    {
        internal void ConfigureMapping()
        {
            serviceCollection.AddScoped<NotificationMapper>();
        }
    }
}
