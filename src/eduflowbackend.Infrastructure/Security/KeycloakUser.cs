using System.Text.Json.Serialization;

namespace eduflowbackend.Infrastructure.Security;

internal class KeycloakUser
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
}