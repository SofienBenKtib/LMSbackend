using eduflowbackend.Application.dtos;
using FluentResults;
using Mediator;

namespace eduflowbackend.Application.Resources.Get;

public record GetAllResourcesQuery : IRequest<Result<List<ResourceDto>>>;
