namespace eduflowbackend.Core.Abstractions;

public interface ICurrentUserService
{
    string? UserId { get; }
}