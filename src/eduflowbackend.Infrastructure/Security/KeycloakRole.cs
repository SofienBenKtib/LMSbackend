using System.Text.Json.Serialization;

namespace eduflowbackend.Infrastructure.Security;

internal class KeycloakRole
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}