using eduflowbackend.Core.Abstractions;
using eduflowbackend.Core.User;
using FluentResults;
using Mediator;
using Microsoft.Extensions.Logging;

namespace eduflowbackend.Application.Users.Get;

public class GetAllUsersHandler : IRequestHandler<GetAllUsersCommand, Result<List<User>>>
{
    private readonly IRepository<User> _repository;
    // private readonly ILogger<GetAllUsersHandler> _logger;

    public GetAllUsersHandler(IRepository<User> repository)
    {
        _repository = repository;
    }

    public async ValueTask<Result<List<User>>> Handle(GetAllUsersCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var users = await _repository.GetAllAsync(cancellationToken);
            return Result.Ok(users);
        }
        catch (Exception ex)
        {
            return Result.Fail<List<User>>("Failed to get users" + ex.Message);
        }
    }
}