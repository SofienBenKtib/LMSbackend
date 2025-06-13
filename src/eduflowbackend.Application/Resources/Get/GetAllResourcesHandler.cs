using eduflowbackend.Application.dtos;
using eduflowbackend.Core.Abstractions;
using FluentResults;
using Mediator;

namespace eduflowbackend.Application.Resources.Get;

public class GetAllResourcesHandler : IRequestHandler<GetAllResourcesQuery, Result<List<ResourceDto>>>
{
    private readonly IRepository<ResourceDto> _repository;

    public GetAllResourcesHandler(IRepository<ResourceDto> repository)
    {
        _repository = repository;
    }

    public async ValueTask<Result<List<ResourceDto>>> Handle(GetAllResourcesQuery request,
        CancellationToken cancellationToken)
    {
        var resources = await _repository.GetAllAsync(cancellationToken);
        return Result.Ok(resources);
    }
}