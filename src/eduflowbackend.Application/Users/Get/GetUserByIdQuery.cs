using eduflowbackend.Core.User;
using Mediator;

namespace eduflowbackend.Application.Users.Get;

public record GetUserByIdQuery(Guid Id):IRequest<User>;