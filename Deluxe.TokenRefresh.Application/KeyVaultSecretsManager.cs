using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Deluxe.TokenRefresh.Domain.Configuration;

namespace Deluxe.TokenRefresh.Application;

public class KeyVaultSecretsManager : IKeyVaultSecretsManager
{
    private readonly KeyVaultOptions _settings;
    public KeyVaultSecretsManager(KeyVaultOptions settings)
    {
        _settings = settings;
    }

    public async Task<string> GetSecretFromVault(string secretKeyIdentifier)
    {
        var client = ConnectToKeyVault();

        var secret = await client.GetSecretAsync(secretKeyIdentifier).ConfigureAwait(false);

        return secret.Value.Value.ToString();
    }

    public async Task<string> UpdateSecretAttributes(string secretKeyIdentifier, string secretValue, DateTime expired)
    {
        var client = ConnectToKeyVault(); 

        KeyVaultSecret oldSecret = await client.GetSecretAsync(secretKeyIdentifier);
        oldSecret.Properties.Enabled = false;

        KeyVaultSecret secret = await client.SetSecretAsync(secretKeyIdentifier, secretValue);
        secret.Properties.ExpiresOn = expired;

        _ = await client.UpdateSecretPropertiesAsync(secret.Properties);
        _ = await client.UpdateSecretPropertiesAsync(oldSecret.Properties);

        return secret.Value;
    }
    private SecretClient ConnectToKeyVault()
    {
        var client = new SecretClient(new Uri(_settings.KeyVaultUri), new DefaultAzureCredential());
        return client;
    }
}