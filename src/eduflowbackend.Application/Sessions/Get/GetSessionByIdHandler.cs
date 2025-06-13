using eduflowbackend.Application.dtos;
using eduflowbackend.Core.Abstractions;
using eduflowbackend.Core.Exceptions;
using eduflowbackend.Core.Session;
using FluentResults;
using Mediator;

namespace eduflowbackend.Application.Sessions.Get;

public class GetSessionByIdHandler : IRequestHandler<GetSessionByIdQuery, Result<SessionDto>>
{
    private readonly IRepository<Session> _repository;

    public GetSessionByIdHandler(IRepository<Session> repository)
    {
        _repository = repository;
    }

    public async ValueTask<Result<SessionDto>> Handle(GetSessionByIdQuery request, CancellationToken cancellationToken)
    {
        //  Checking the existence of the session in the DB
        var session = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (session == null)
        {
            return Result.Fail<SessionDto>(new NotFoundError(nameof(Session), request.Id.ToString()));
        }

        var sessionDto = new SessionDto
        {
            Id = session.Id,
            Title = session.Title,
            Description = session.Description,
            StartDate = session.StartDate,
            EndDate = session.EndDate,
            InstructorId = session.InstructorId,
        };
        return Result.Ok(sessionDto);
    }
}