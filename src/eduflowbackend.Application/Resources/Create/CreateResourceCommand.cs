using FluentResults;
using Mediator;

namespace eduflowbackend.Application.Resources.Create;

/// <summary>
/// This class is used to retrieve the data from the client
/// </summary>
public class CreateResourceRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

/// <summary>
/// This class represents a command in the CQRS pattern
/// </summary>
public class CreateResourceCommand : IRequest<Result<Guid>>
{
    public string Title { get; set; }
    public string Description { get; set; }

    public CreateResourceCommand(string title, string description)
    {
        Title = title;
        Description = description;
    }
}