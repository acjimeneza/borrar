using Deluxe.TokenRefresh.Domain;
using Deluxe.TokenRefresh.Infrastructure.Deluxe;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Deluxe.TokenRefresh.Application.Tests;

[TestClass]
public class DeluxeTokenRefreshServiceTests
{
    private IDeluxeDPXApi _deluxeDPXApi;
    private IKeyVaultSecretsManager _keyVaultSecretsManager;
    private IDeluxeTokenRefreshService _sut;

    [TestInitialize]
    public void Init()
    {
        _deluxeDPXApi = Substitute.For<IDeluxeDPXApi>();
        _keyVaultSecretsManager = Substitute.For<IKeyVaultSecretsManager>();

        _sut = new DeluxeTokenRefreshService(_keyVaultSecretsManager, _deluxeDPXApi);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public async Task RefreshToken_ThrowsErrorWhenOldTokenIsNull()
    {
        var secretKey = "secretKey";

        _ =_keyVaultSecretsManager.GetSecretFromVault(secretKey).Returns(string.Empty);

       await _sut.RefreshToken(secretKey, CancellationToken.None);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public async Task RefreshToken_ThrowsErrorWhenNewTokenIsNull()
    {
        var secretKey = "secretKey";
        var oldToken = "oldToken";

        _ =_keyVaultSecretsManager.GetSecretFromVault(secretKey).Returns(oldToken);
        _deluxeDPXApi.RefreshTokenAsync(oldToken).Returns(Task.FromResult<Token?>(null));

        await _sut.RefreshToken(secretKey, CancellationToken.None);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public async Task RefreshToken_ThrowsErrorWhenUpdateSecretAttributesReturnsEmpty()
    {
        var secretKey = "secretKey";
        var oldToken = "oldToken";
        var token = new Token
        {
            TokenAuth = "newToken",
            ExpiresAt = DateTime.UtcNow.AddDays(10) 
        };

        _ =_keyVaultSecretsManager.GetSecretFromVault(secretKey).Returns(oldToken);
        _=_deluxeDPXApi.RefreshTokenAsync(oldToken).Returns(token);
        _=_keyVaultSecretsManager.UpdateSecretAttributes(secretKey, token.TokenAuth, token.ExpiresAt).Returns(string.Empty);

        await _sut.RefreshToken(secretKey, CancellationToken.None);
    }

    [TestMethod]
    public async Task RefreshToken_Successfully()
    {
        var secretKey = "secretKey";
        var oldToken = "oldToken";
        var token = new Token
        {
            TokenAuth = "newToken",
            ExpiresAt = DateTime.UtcNow.AddDays(10)
        };

        _ =_keyVaultSecretsManager.GetSecretFromVault(secretKey).Returns(oldToken);
        _=_deluxeDPXApi.RefreshTokenAsync(oldToken).Returns(token);
        _=_keyVaultSecretsManager.UpdateSecretAttributes(secretKey, token.TokenAuth, token.ExpiresAt).Returns(token.TokenAuth);

        await _sut.RefreshToken(secretKey, CancellationToken.None);

        _keyVaultSecretsManager.Received(2);
        _deluxeDPXApi.Received(1);
    }
}