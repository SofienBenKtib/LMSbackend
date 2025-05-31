using eduflowbackend.Application.dtos;
using eduflowbackend.Core.Abstractions;
using eduflowbackend.Core.User;
using FluentResults;
using Mediator;
using Microsoft.Extensions.Logging;

namespace eduflowbackend.Application.Users.Get;

public class GetAllUsersHandler : IRequestHandler<GetAllUsersCommand, Result<List<UserDTO>>>
{
    private readonly IRepository<UserDTO> _repository;
    // private readonly ILogger<GetAllUsersHandler> _logger;

    public GetAllUsersHandler(IRepository<UserDTO> repository)
    {
        _repository = repository;
    }

    public async ValueTask<Result<List<UserDTO>>> Handle(GetAllUsersCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var users = await _repository.GetAllAsync(cancellationToken);
            return Result.Ok(users);
        }
        catch (Exception ex)
        {
            return Result.Fail<List<UserDTO>>("Failed to get users" + ex.Message);
        }
    }
}