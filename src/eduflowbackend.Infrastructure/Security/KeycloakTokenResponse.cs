using System.Text.Json.Serialization;

namespace eduflowbackend.Infrastructure.Security;

internal class KeycloakTokenResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = string.Empty;
}