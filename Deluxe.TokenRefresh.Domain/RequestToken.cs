using System.Text.Json.Serialization;

namespace Deluxe.TokenRefresh.Domain;

public class RequestToken
{
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;
}
