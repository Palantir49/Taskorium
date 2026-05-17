using Microsoft.Extensions.Hosting;

namespace Taskorium.ServiceDefaults;

public static class HostBuilderExtensions
{
    extension(IHostBuilder hostBuilder)
    {
        public void ValidateServices()
        {
            hostBuilder.UseDefaultServiceProvider((_, options) =>
            {
                options.ValidateOnBuild = true;
                options.ValidateScopes = true;
            });
        }
    }
}
