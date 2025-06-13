using FluentResults;
using Mediator;

namespace eduflowbackend.Application.Sessions.Delete;

public class DeleteSessionCommand : IRequest<Result<Guid>>
{
    public DeleteSessionCommand(Guid sessionId)
    {
        SessionId = sessionId;
    }
    
    public Guid SessionId { get; set; }
}