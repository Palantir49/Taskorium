using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace Taskorium.ServiceDefaults;

public static class ConfigurationExtensions
{
    extension(ConfigurationManager configurationManager)
    {
        public void Setup(string environmentName)
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            var configurationBuilder = configurationManager.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{environmentName}.json", true)
                .AddEnvironmentVariables();
            if (entryAssembly is not null)
            {
                configurationBuilder.AddUserSecrets(entryAssembly);
            }

            configurationBuilder.Build();
        }
    }
}
