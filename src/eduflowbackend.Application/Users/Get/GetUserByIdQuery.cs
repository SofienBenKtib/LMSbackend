using eduflowbackend.Application.dtos;
using FluentResults;
using Mediator;

namespace eduflowbackend.Application.Users.Get;

public record GetUserByIdQuery(Guid Id) : IRequest<Result<UserDto>>;