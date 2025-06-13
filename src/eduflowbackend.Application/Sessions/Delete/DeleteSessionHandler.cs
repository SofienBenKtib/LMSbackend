using eduflowbackend.Core.Abstractions;
using eduflowbackend.Core.Session;
using FluentResults;
using Mediator;

namespace eduflowbackend.Application.Sessions.Delete;

public class DeleteSessionHandler : IRequestHandler<DeleteSessionCommand, Result<Guid>>
{
    private readonly IRepository<Session> _repository;

    public DeleteSessionHandler(IRepository<Session> repository)
    {
        _repository = repository;
    }

    public async ValueTask<Result<Guid>> Handle(DeleteSessionCommand request, CancellationToken cancellationToken)
    {
        //  Finding the session in the DB
        var session = await _repository.GetByIdAsync(request.SessionId);


        //  Deleting the session
        await _repository.DeleteAsync(session);
        return Result.Ok();
    }
}