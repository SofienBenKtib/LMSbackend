using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using FluentResults;
using Mediator;


namespace eduflowbackend.Application.Queries;

public class UpdateUserCommand:IRequest<Result>
{
    public Guid Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    [EmailAddress]
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
}