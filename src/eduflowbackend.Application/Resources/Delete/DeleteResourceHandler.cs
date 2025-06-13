using eduflowbackend.Core.Abstractions;
using eduflowbackend.Core.Resource;
using FluentResults;
using Mediator;

namespace eduflowbackend.Application.Resources.Delete;

public class DeleteResourceHandler : IRequestHandler<DeleteResourceCommand, Result<Guid>>
{
    private readonly IRepository<Resource> _repository;

    public DeleteResourceHandler(IRepository<Resource> repository)
    {
        _repository = repository;
    }

    public async ValueTask<Result<Guid>> Handle(DeleteResourceCommand request, CancellationToken cancellationToken)
    {
        //  Retrieve the resource from the 
        var resource = await _repository.GetByIdAsync(request.ResourceId);
        //  Deleting the session
        await _repository.DeleteAsync(resource);
        return Result.Ok();
    }
}