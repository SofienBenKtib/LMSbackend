using System.ComponentModel.DataAnnotations;

namespace eduflowbackend.Infrastructure.Security;

public class KeycloakConfig
{
    public const string SectionName = "Keycloak";
    [Required]
    public string BaseUrl { get; set; } = string.Empty;

    [Required]
    public string Realm { get; set; } = string.Empty;

    [Required]
    public string ClientId { get; set; } = string.Empty;

    [Required]
    public string ClientSecret { get; set; } = string.Empty;
}