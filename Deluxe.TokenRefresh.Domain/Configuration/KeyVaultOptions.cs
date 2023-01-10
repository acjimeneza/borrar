using Deluxe.TokenRefresh.Domain.Enums;

namespace Deluxe.TokenRefresh.Domain.Configuration;

public class KeyVaultOptions
{
    public KeyVaultUsage Mode { get; set; }
    public string KeyVaultUri { get; set; } = string.Empty;
}
