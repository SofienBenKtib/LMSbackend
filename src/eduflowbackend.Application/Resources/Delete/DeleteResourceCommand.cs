using FluentResults;
using Mediator;

namespace eduflowbackend.Application.Resources.Delete;

public class DeleteResourceCommand : IRequest<Result<Guid>>
{
    public Guid ResourceId { get; set; }

    public DeleteResourceCommand(Guid resourceId)
    {
        ResourceId = resourceId;
    }
}