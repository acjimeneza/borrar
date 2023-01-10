using Deluxe.TokenRefresh.Domain;

namespace Deluxe.TokenRefresh.Infrastructure.Deluxe;

public interface IDeluxeDPXApi
{
    Task<Token?> GetRequestTokenAsync(string email, CancellationToken cancellationToken = default);
    Task<Token?> ResolveRequestTokenAsync(string token, CancellationToken cancellationToken = default);
    Task<Token?> RefreshTokenAsync(string token, CancellationToken cancellationToken = default);
}
