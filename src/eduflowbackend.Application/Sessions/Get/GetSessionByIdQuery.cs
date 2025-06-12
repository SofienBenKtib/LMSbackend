using eduflowbackend.Application.dtos;
using FluentResults;
using Mediator;

namespace eduflowbackend.Application.Sessions.Get;

public record GetSessionByIdQuery(Guid Id) : IRequest<Result<SessionDto>>;