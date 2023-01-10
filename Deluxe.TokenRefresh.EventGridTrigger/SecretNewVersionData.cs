using System.Text.Json.Serialization;

namespace Deluxe.TokenRefresh.EventGridTrigger
{
    public class SecretNewVersionData
    {
        [JsonPropertyName("vaultName")]
        public string VaultName { get; set; } = string.Empty;
        [JsonPropertyName("vaultName")]
        public string ObjectType { get; set; } = string.Empty;
        [JsonPropertyName("vaultName")]
        public string ObjectName { get; set; } = string.Empty;
    }
}
