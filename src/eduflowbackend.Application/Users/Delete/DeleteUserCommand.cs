using FluentResults;
using Mediator;

namespace eduflowbackend.Application.Queries;

public class DeleteUserCommand : IRequest<Result<Guid>>
{
    public DeleteUserCommand(Guid userId)
    {
        userId = userId;
    }

    public Guid UserId { get; set; }
}