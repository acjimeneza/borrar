using System.Text.Json.Serialization;

namespace Deluxe.TokenRefresh.Domain;

public class ErrorResponse
{
    [JsonPropertyName("error")]
    public string Error { get; set; } = string.Empty;
    [JsonPropertyName("errors")]
    public List<string>? Errors { get; set; } = default!;
    [JsonPropertyName("error_data")]
    public Dictionary<string, List<string>>? ErrorData { get; set; } = default!;
}
