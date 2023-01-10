using Deluxe.TokenRefresh.Domain.Configuration;
using Deluxe.TokenRefresh.Infrastructure.Deluxe;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Deluxe.TokenRefresh.Infrastructure.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class DependencyInjectionRegistrations
    {
        private const string DeluxeApiSection = "Vendors:Deluxe";
        private const string KeyVaultApiSection = "Secrets";
        public static IServiceCollection AddInfraestructure(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(provider =>
            {
                var deluxeApiSettings = new DeluxeVendorSettings();
                var configuration = provider.GetRequiredService<IConfiguration>();
                configuration.Bind(DeluxeApiSection, deluxeApiSettings);

                return deluxeApiSettings;
            });

            serviceCollection.AddSingleton(provider =>
            {
                var keyVaultOptions = new KeyVaultOptions();
                var configuration = provider.GetRequiredService<IConfiguration>();
                configuration.Bind(KeyVaultApiSection, keyVaultOptions);

                return keyVaultOptions;
            });

            serviceCollection.AddTransient<IDeluxeDPXApi, DeluxeDPXApi>();

            return serviceCollection;
        }

    }
}
