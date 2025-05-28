using eduflowbackend.Application.Users.Create;
using FluentResults;

namespace eduflowbackend.Application.Abstractions;

public interface IIdentityProviderService
{
    Task<Result<(string userId, string temporaryPassword)>> CreateUserAsync(CreateUserCommand command);
    Task<Result> DeleteUserAsync(string userId);
    Task<Result> AssignUserToRoleAsync(string userId, string? roleName);
}