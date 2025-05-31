using eduflowbackend.Core.User;
using FluentResults;
using Mediator;

namespace eduflowbackend.Application.Users.Get;

public class GetAllUsersCommand : IRequest<Result<List<User>>>
{
    
}