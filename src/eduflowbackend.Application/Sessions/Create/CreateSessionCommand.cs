using eduflowbackend.Core.Session;
using FluentResults;
using Mediator;

namespace eduflowbackend.Application.Sessions.Create;

/*/// <summary>
/// This class is a DTO that's used to retrieve data from the front-end
/// </summary>*/
/*public class CreateSessionRequest
{
    public string Link { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
}*/
/*/// <summary>
/// This class represents a command in the CQRS pattern
/// </summary>*/
/*public class CreateSessionCommand : IRequest<Result<Guid>>
{
    public CreateSessionCommand(string link, DateTime startDate)
    {
        Link = link;
        StartDate = startDate;
    }
    public string Link { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }

}*/

public record CreateSessionCommand(string Link) : IRequest<Session>;