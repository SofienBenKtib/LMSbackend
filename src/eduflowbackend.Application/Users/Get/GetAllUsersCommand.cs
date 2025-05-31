using eduflowbackend.Application.dtos;
using FluentResults;
using Mediator;

namespace eduflowbackend.Application.Users.Get;

public class GetAllUsersCommand : IRequest<Result<List<UserDto>>>
{
}