using FluentResults;
using Mediator;

namespace eduflowbackend.Application.Sessions.Create;

/// <summary>
/// This class is a DTO that's used to retrieve data from the front-end
/// </summary>
public class CreateSessionRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

/// <summary>
/// This class represents a command in the CQRS pattern
/// </summary>
public class CreateSessionCommand : IRequest<Result<Guid>>
{
    public CreateSessionCommand(string title, string description)
    {
        Title = title;
        Description = description;
    }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}