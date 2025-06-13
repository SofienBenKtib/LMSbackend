using eduflowbackend.Application.dtos;
using FluentResults;
using Mediator;

namespace eduflowbackend.Application.Resources.Get;

public record GetResourceByIdQuery(Guid Id) : IRequest<Result<ResourceDto>>;