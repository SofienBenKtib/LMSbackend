using eduflowbackend.Core.Abstractions;
using eduflowbackend.Core.Resource;
using FluentResults;
using Mediator;

namespace eduflowbackend.Application.Resources.Create;

public class CreateResourceHandler : IRequestHandler<CreateResourceCommand, Result<Guid>>
{
    private readonly IRepository<Resource> _repository;

    public CreateResourceHandler(IRepository<Resource> repository)
    {
        _repository = repository;
    }

    public async ValueTask<Result<Guid>> Handle(CreateResourceCommand request, CancellationToken cancellationToken)
    {
        //  Create a new resource
        var resource = new Resource(request.Title, request.Description);
        //  Persist to repository
        await _repository.AddAsync(resource, cancellationToken);
        //  Return the resource's Id
        return Result.Ok();
    }
}