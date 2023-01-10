namespace Deluxe.TokenRefresh.Application
{
    public interface IDeluxeTokenRefreshService
    {
        Task RefreshToken(string secretKeyIdentifier, CancellationToken cancellationToken);
    }
}
