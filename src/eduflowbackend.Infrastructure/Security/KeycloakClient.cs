using System.Text.Json.Serialization;

namespace eduflowbackend.Infrastructure.Security;

internal class KeycloakClient
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    [JsonPropertyName("clientId")]
    public string ClientId { get; set; }
}