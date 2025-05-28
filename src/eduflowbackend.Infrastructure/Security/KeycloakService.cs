using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using eduflowbackend.Application.Abstractions;
using eduflowbackend.Application.Users.Create;
using FluentResults;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace eduflowbackend.Infrastructure.Security;

public class KeycloakService : IIdentityProviderService
{
    private readonly HttpClient _httpClient;
    private readonly KeycloakConfig _keycloakConfig;
    private readonly ILogger<KeycloakService> _logger;

    public KeycloakService(HttpClient httpClient, IOptions<KeycloakConfig> keycloakConfig, ILogger<KeycloakService> logger)
    {
        _httpClient = httpClient;
        _keycloakConfig = keycloakConfig.Value;
        _logger = logger;
    }

    /// <summary>
    /// Creates a user in Keycloak and assigns a specified role.
    /// </summary>
    public async Task<Result<(string, string)>> CreateUserAsync(CreateUserCommand command)
    {
        try
        {
            var tokenResult = await GetAdminAccessTokenAsync();
            if (tokenResult.IsFailed) return Result.Fail(tokenResult.Errors.First());

            var token = tokenResult.Value;
            var temporaryPassword = TemporaryPasswordGenerator.Generate();

            var createUserResult = await CreateKeycloakUserAsync(command, temporaryPassword, token);
            if (createUserResult.IsFailed) return Result.Fail(createUserResult.Errors.First());
            
            return Result.Ok((createUserResult.Value, temporaryPassword));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while creating Keycloak user.");
            return Result.Fail<(string, string)>("An unexpected error occurred.");
        }
    }

    public async Task<Result> DeleteUserAsync(string userId)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"{_keycloakConfig}/admin/realms/{_keycloakConfig.Realm}/users/{userId}");
        var response = await _httpClient.SendAsync(request);
        return response.IsSuccessStatusCode ? Result.Ok() : Result.Fail("Failed to delete Keycloak user");
    }

    public async Task<Result> AssignUserToRoleAsync(string userId, string roleName)
    {
        try
        {
            var tokenResult = await GetAdminAccessTokenAsync();
            if (tokenResult.IsFailed) return Result.Fail(tokenResult.Errors.First());

            var token = tokenResult.Value;
            var roleResult = await GetRoleByNameAsync(roleName, token);
            if (roleResult.IsFailed) return roleResult.ToResult();

            var role = roleResult.Value;
            var request = new HttpRequestMessage(HttpMethod.Post,
                $"{_keycloakConfig.BaseUrl}/admin/realms/{_keycloakConfig.Realm}/users/{userId}/role-mappings/realm")
            {
                Content =
                    new StringContent(JsonSerializer.Serialize(new[] { role }), Encoding.UTF8, "application/json"),
                Headers = { Authorization = new AuthenticationHeaderValue("Bearer", token) }
            };

            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode ? Result.Ok() : Result.Fail("Failed to assign role.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while creating Keycloak user.");
            return Result.Fail("An unexpected error occurred.");
        }
    }

    private async Task<Result<string>> GetAdminAccessTokenAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{_keycloakConfig.BaseUrl}/realms/master/protocol/openid-connect/token")
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "client_id", _keycloakConfig.ClientId },
                { "client_secret", _keycloakConfig.ClientSecret },
                { "grant_type", "client_credentials" }
            })
        };

        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
            return Result.Fail<string>("Failed to get Keycloak admin token.");

        var content = await response.Content.ReadAsStringAsync();
        var tokenData = JsonSerializer.Deserialize<KeycloakTokenResponse>(content);
        return tokenData?.AccessToken != null ? Result.Ok(tokenData.AccessToken) : Result.Fail("Invalid token response.");
    }

    private async Task<Result<string>> CreateKeycloakUserAsync(CreateUserCommand userDto, string temporaryPassword, string token)
    {
        var userPayload = new
        {
            username = userDto.Email,
            firstName = userDto.Firstname,
            lastName = userDto.Lastname,
            email = userDto.Email,
            enabled = true,
            credentials = new[] { new { type = "password", value = temporaryPassword, temporary = true } }
        };

        var request = new HttpRequestMessage(HttpMethod.Post, $"{_keycloakConfig.BaseUrl}/admin/realms/{_keycloakConfig.Realm}/users")
        {
            Content = new StringContent(JsonSerializer.Serialize(userPayload), Encoding.UTF8, "application/json"),
            Headers = { Authorization = new AuthenticationHeaderValue("Bearer", token) }
        };

        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
            return Result.Fail<string>($"Failed to create user: {response.ReasonPhrase}");
        return await GetUserIdByUsernameAsync(userDto.Email, token);
    }

    private async Task<Result<string>> GetUserIdByUsernameAsync(string username, string token)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{_keycloakConfig.BaseUrl}/admin/realms/{_keycloakConfig.Realm}/users?username={username}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
            return Result.Fail("Failed to retrieve user ID.");

        var content = await response.Content.ReadAsStringAsync();
        var users = JsonSerializer.Deserialize<List<KeycloakUser>>(content);
        return users?.FirstOrDefault()?.Id ?? Result.Fail<string>("User ID not found.");
    }
    

    private async Task<Result<KeycloakRole>> GetRoleByNameAsync(string roleName, string token)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{_keycloakConfig.BaseUrl}/admin/realms/{_keycloakConfig.Realm}/roles/{roleName}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
            return Result.Fail<KeycloakRole>($"Role '{roleName}' not found.");

        var content = await response.Content.ReadAsStringAsync();
        var role = JsonSerializer.Deserialize<KeycloakRole>(content);
        return role != null ? Result.Ok(role) : Result.Fail<KeycloakRole>("Invalid role response.");
    }
    
    private async Task<Result<string>> GetInternalClientIdAsync(string token)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, 
            $"{_keycloakConfig.BaseUrl}/admin/realms/{_keycloakConfig.Realm}/clients?clientId={_keycloakConfig.ClientId}");
    
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
            return Result.Fail<string>($"Failed to retrieve internal Client ID for '{_keycloakConfig.ClientId}'.");

        var content = await response.Content.ReadAsStringAsync();
        var clients = JsonSerializer.Deserialize<List<KeycloakClient>>(content);
    
        var client = clients?.FirstOrDefault();
        return client?.Id != null ? Result.Ok(client.Id) : Result.Fail<string>("Client ID not found.");
    }
}