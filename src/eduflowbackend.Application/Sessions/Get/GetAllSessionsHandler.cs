using System.Collections.Concurrent;
using eduflowbackend.Application.dtos;
using eduflowbackend.Core.Abstractions;
using eduflowbackend.Core.Session;
using FluentResults;
using Mediator;

namespace eduflowbackend.Application.Sessions.Get;

public class GetAllSessionsHandler : IRequestHandler<GetAllSessionsQuery, IEnumerable<Session>>
{
    private readonly IRepository<Session> _repository;

    public GetAllSessionsHandler(IRepository<Session> repository)
    {
        _repository = repository;
    }

    public ValueTask<IEnumerable<Session>> Handle(GetAllSessionsQuery request,
        CancellationToken cancellationToken)
    {
        var sessions = SessionStorage.Sessions.Values
            .OrderByDescending(s => s.StartDate)
            .ToList();
        
        return ValueTask.FromResult<IEnumerable<Session>>(sessions);
    }
}