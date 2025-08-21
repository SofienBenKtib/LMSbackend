using System.ComponentModel.DataAnnotations;

namespace eduflowbackend.Application.Users.Update;

public class UpdateUserRequest
{
    public Guid Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    [EmailAddress]
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
}