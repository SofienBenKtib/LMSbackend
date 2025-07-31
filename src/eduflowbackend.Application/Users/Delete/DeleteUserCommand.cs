using FluentResults;
using Mediator;

namespace eduflowbackend.Application.Queries;

public class DeleteUserCommand : IRequest<Result<Guid>>
{
    public DeleteUserCommand(Guid userId)
    {
        UserId = userId;
    }

    public Guid UserId { get; set; }
}