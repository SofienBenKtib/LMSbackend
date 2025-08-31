using eduflowbackend.Core.Abstractions;
using eduflowbackend.Core.Session;
using FluentResults;
using Mediator;

namespace eduflowbackend.Application.Sessions.Delete;

public class DeleteSessionHandler : IRequestHandler<DeleteSessionCommand, bool>
{
    private readonly IRepository<Session> _repository;

    public DeleteSessionHandler(IRepository<Session> repository)
    {
        _repository = repository;
    }

    public async ValueTask<bool> Handle(DeleteSessionCommand request, CancellationToken cancellationToken)
    {
        return SessionStorage.Sessions.TryRemove(request.Id, out _);
    }
}