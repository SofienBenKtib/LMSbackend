using FluentResults;
using Mediator;

namespace eduflowbackend.Application.Sessions.Delete;

public record DeleteSessionCommand(Guid Id) : IRequest<bool>;