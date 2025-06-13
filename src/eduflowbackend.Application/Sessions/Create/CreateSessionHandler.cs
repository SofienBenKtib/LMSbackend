using eduflowbackend.Core.Abstractions;
using eduflowbackend.Core.Session;
using FluentResults;
using Mediator;

namespace eduflowbackend.Application.Sessions.Create;

public class CreateSessionHandler : IRequestHandler<CreateSessionCommand, Result<Guid>>
{
    private readonly IRepository<Session> _repository;

    public CreateSessionHandler(IRepository<Session> repository)
    {
        _repository = repository;
    }

    public async ValueTask<Result<Guid>> Handle(CreateSessionCommand request, CancellationToken cancellationToken)
    {
        //  Create a new session
        var session = new Session(request.Title, request.Description);

        //  Persist to repository
        await _repository.AddAsync(session, cancellationToken);

        //  Return the session's Id
        return Result.Ok();
    }
}