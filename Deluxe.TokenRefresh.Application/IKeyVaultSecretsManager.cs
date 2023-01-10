namespace Deluxe.TokenRefresh.Application;

public interface IKeyVaultSecretsManager
{
    Task<string> GetSecretFromVault(string secretKeyIdentifier);
    Task<string> UpdateSecretAttributes(string secretKeyIdentifier, string secretValue, DateTime expired);
}