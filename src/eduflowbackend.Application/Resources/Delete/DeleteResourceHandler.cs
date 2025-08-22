using eduflowbackend.Core.Abstractions;
using eduflowbackend.Core.Exceptions;
using eduflowbackend.Core.Resource;
using FluentResults;
using Mediator;

namespace eduflowbackend.Application.Resources.Delete;

public class DeleteResourceHandler : IRequestHandler<DeleteResourceCommand, Result>
{
    private readonly IRepository<Resource> _repository;

    public DeleteResourceHandler(IRepository<Resource> repository)
    {
        _repository = repository;
    }

    public async ValueTask<Result> Handle(DeleteResourceCommand request, CancellationToken cancellationToken)
    {
        var resource = await _repository.GetByIdAsync(request.ResourceId, cancellationToken);
        if (resource != null) 
        {
            await _repository.DeleteAsync(resource, cancellationToken);
        }
        return Result.Ok();
    }
}