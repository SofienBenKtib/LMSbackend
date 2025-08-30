using System.Collections.Concurrent;
using eduflowbackend.Core.Abstractions;
using eduflowbackend.Core.Session;
using FluentResults;
using Mediator;

namespace eduflowbackend.Application.Sessions.Create;

// Separate DTO for API input
public class CreateSessionRequest
{
    public string Link { get; set; } = string.Empty;
}

public class CreateSessionHandler : IRequestHandler<CreateSessionCommand, Session>

{
    public ValueTask<Session> Handle(CreateSessionCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Link))
        {
            throw new ArgumentException("Link cannot be null or empty");
        }

        var session = new Session
        {
            Id = Guid.NewGuid(),
            Link = request.Link.Trim(),
            StartDate = DateTime.UtcNow
        };


        //_sessions.TryAdd(session.Id, session);
        SessionStorage.Sessions.TryAdd(session.Id, session);
        return ValueTask.FromResult(session);
    }
}