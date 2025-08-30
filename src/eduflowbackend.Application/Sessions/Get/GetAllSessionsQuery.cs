using eduflowbackend.Application.dtos;
using eduflowbackend.Core.Session;
using FluentResults;
using Mediator;

namespace eduflowbackend.Application.Sessions.Get;

public record GetAllSessionsQuery() : IRequest<IEnumerable<Session>>;