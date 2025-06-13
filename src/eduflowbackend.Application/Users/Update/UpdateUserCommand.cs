using FluentResults;
using Mediator;

namespace eduflowbackend.Application.Queries;

public class UpdateUserCommand:IRequest<Result<string>>
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}