using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Interfaces.Email;
using NotificationService.Infrastructure.Email.Options;
using NotificationService.Infrastructure.Email.Services;

namespace NotificationService.Infrastructure.Extensions.Email;

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


            serviceCollection.AddScoped<IEmailSenderService, EmailSenderService>();
        }
    }
}
