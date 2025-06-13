using eduflowbackend.Application.dtos;
using eduflowbackend.Core.Abstractions;
using eduflowbackend.Core.Exceptions;
using eduflowbackend.Core.Resource;
using FluentResults;
using Mediator;

namespace eduflowbackend.Application.Resources.Get;

public class GetResourceByIdHandler : IRequestHandler<GetResourceByIdQuery, Result<ResourceDto>>
{
    private readonly IRepository<Resource> _repository;

    public GetResourceByIdHandler(IRepository<Resource> repository)
    {
        _repository = repository;
    }

    public async ValueTask<Result<ResourceDto>> Handle(GetResourceByIdQuery request,
        CancellationToken cancellationToken)
    {
        //  Checking the existence of the resource in the DB
        var resource = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (resource == null)
        {
            return Result.Fail<ResourceDto>(new NotFoundError(nameof(Resource), request.Id.ToString()));
        }

        var resourceDto = new ResourceDto
        {
            Id = resource.Id,
            Title = resource.Title,
            Description = resource.Description,
            CreatedAt = resource.CreatedAt,
            SessionId = resource.SessionId,
            // AddedById = resource.AddedById
        };
        return Result.Ok(resourceDto);
    }
}