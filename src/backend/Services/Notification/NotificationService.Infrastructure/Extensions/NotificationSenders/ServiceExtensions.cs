using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Interfaces.NotificationSenders;
using NotificationService.Infrastructure.NotificationSenders.Options;
using NotificationService.Infrastructure.NotificationSenders.Services;

namespace NotificationService.Infrastructure.Extensions.NotificationSenders;

internal static class ServiceExtensions
{
    extension(IServiceCollection serviceCollection)
    {
        internal void ConfigureEmailSenderService(IConfiguration configuration)
        {
            serviceCollection
                .AddOptions<SmtpOptions>()
                .Bind(configuration.GetSection(SmtpOptions.SectionName))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            serviceCollection.AddScoped<INotificationSender, NotificationSender>();
            serviceCollection.AddScoped<IChannelSender, EmailSenderService>();
        }
    }
}
