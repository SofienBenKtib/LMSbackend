using eduflowbackend.Core.User;
using FluentResults;
using Mediator;

namespace eduflowbackend.Application.Users.Create;

public class CreateUserRequest
{
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public Role Role { get; set; }
}

public class CreateUserCommand : IRequest<Result<Guid>>
{
    public CreateUserCommand(string firstname, string lastname, string email, string phoneNumber, Role role)
    {
        Firstname = firstname;
        Lastname = lastname;
        Email = email;
        PhoneNumber = phoneNumber;
        Role = role;
    }

    public string Firstname { get; }
    public string Lastname { get; }
    public string Email { get; }
    public string PhoneNumber { get; }
    public Role Role { get; }
}