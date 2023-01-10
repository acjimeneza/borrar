using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Deluxe.TokenRefresh.Domain;

[ExcludeFromCodeCoverage]

public class Token
{
    [JsonPropertyName("token")]
    public string TokenAuth { get; set; } = string.Empty;
    [JsonPropertyName("expires_at")]
    public DateTime ExpiresAt { get; set; }
}
