using Azure.Identity;
using Deluxe.TokenRefresh.Application;
using Deluxe.TokenRefresh.Domain.Configuration;
using Deluxe.TokenRefresh.Domain.Enums;
using Deluxe.TokenRefresh.Domain.Utilities;
using Deluxe.TokenRefresh.Infrastructure.Deluxe;
using Deluxe.TokenRefresh.Infrastructure.Extensions;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

[assembly: FunctionsStartup(typeof(Deluxe.TokenRefresh.EventGridTrigger.Startup))]
namespace Deluxe.TokenRefresh.EventGridTrigger;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddTransient<IKeyVaultSecretsManager, KeyVaultSecretsManager>();
        builder.Services.AddTransient<IDeluxeTokenRefreshService, DeluxeTokenRefreshService>();
        builder.Services.AddHttpClient<IDeluxeDPXApi, DeluxeDPXApi>();
        builder.Services.AddInfraestructure();
    }

    public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
    {
        base.ConfigureAppConfiguration(builder);

        FunctionsHostBuilderContext context = builder.GetContext();

        builder.ConfigurationBuilder
            .AddJsonFile(Path.Combine(context.ApplicationRootPath, "appsettings.json"), optional: true, reloadOnChange: false)
            .AddJsonFile(Path.Combine(context.ApplicationRootPath, $"appsettings.{context.EnvironmentName}.json"), optional: true, reloadOnChange: false)
            .AddEnvironmentVariables();

        var configuration = builder.GetContext().Configuration;
        if (configuration["Secrets:Mode"].ToEnum<KeyVaultUsage>() == KeyVaultUsage.UseLocalSecretStore)
        {
            return;
        }

        var keyVaultOptions = configuration.GetSection("Secrets").Get<KeyVaultOptions>();

        builder.ConfigurationBuilder.AddAzureKeyVault(new Uri(keyVaultOptions.KeyVaultUri), new DefaultAzureCredential());
    }
}
