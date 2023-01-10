using Deluxe.TokenRefresh.Infrastructure.Deluxe;

namespace Deluxe.TokenRefresh.Application;

public class DeluxeTokenRefreshService : IDeluxeTokenRefreshService
{
    private readonly IDeluxeDPXApi _deluxeDPXApi;
    private readonly IKeyVaultSecretsManager _keyVaultSecretsManager;

    public DeluxeTokenRefreshService(IKeyVaultSecretsManager keyVaultSecretsManager, IDeluxeDPXApi deluxeDPXApi) 
    {
        _deluxeDPXApi = deluxeDPXApi;
        _keyVaultSecretsManager = keyVaultSecretsManager;
    }

    public async Task RefreshToken(string secretKeyIdentifier, CancellationToken cancellationToken)
    {
        /*var token = await _keyVaultSecretsManager.GetSecretFromVault(secretKeyIdentifier);

        if (string.IsNullOrEmpty(token)) 
        {
            throw new Exception("Error in RefreshToken when trying to get the old token.");
        }*/

        var newToken = await _deluxeDPXApi.RefreshTokenAsync("jKOfXzZy6O1ZqhD7BmnA", cancellationToken);

        if (newToken == null)
        {
            throw new Exception("Error in RefreshToken when trying to refresh the token in Deluxe.");
        }
        else
        {
            throw new Exception($"new token {newToken.TokenAuth}.");
        }

        /*var newSecret = await _keyVaultSecretsManager.UpdateSecretAttributes(secretKeyIdentifier, newToken.TokenAuth, newToken.ExpiresAt.AddDays(-7));

        if (string.IsNullOrEmpty(newSecret))
        {
            throw new Exception("Error in RefreshToken - The new token was not updated in the key vault.");
        }*/
    }
}
