using eduflowbackend.Application.dtos;
using FluentResults;
using Mediator;

namespace eduflowbackend.Application.Sessions.Get;

public record GetAllSessionsQuery : IRequest<Result<List<SessionDto>>>;