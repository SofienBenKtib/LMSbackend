using eduflowbackend.Application.dtos;
using eduflowbackend.Core.Abstractions;
using FluentResults;
using Mediator;

namespace eduflowbackend.Application.Sessions.Get;

public class GetAllSessionsHandler : IRequestHandler<GetAllSessionsQuery, Result<List<SessionDto>>>
{
    private readonly IRepository<SessionDto> _repository;

    public GetAllSessionsHandler(IRepository<SessionDto> repository)
    {
        _repository = repository;
    }

    public async ValueTask<Result<List<SessionDto>>> Handle(GetAllSessionsQuery request,
        CancellationToken cancellationToken)
    {
            var sessions = await _repository.GetAllAsync(cancellationToken);
            return Result.Ok(sessions);
    }
}