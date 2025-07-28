using eduflowbackend.Application.dtos;
using eduflowbackend.Core.Abstractions;
using eduflowbackend.Core.User;
using FluentResults;
using Mediator;

namespace eduflowbackend.Application.Users.Get;

public class GetAllUsersHandler : IRequestHandler<GetAllUsersCommand, Result<List<UserDto>>>
{
    private readonly IRepository<User> _repository;
    //  private readonly IRepository<UserDto> _repository;
    // private readonly ILogger<GetAllUsersHandler> _logger;

    public GetAllUsersHandler(IRepository<User> repository)
    {
        _repository = repository;
    }

    public async ValueTask<Result<List<UserDto>>> Handle(GetAllUsersCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var users = await _repository.GetAllAsync(cancellationToken);
            var userDtos = users.Select(u => new UserDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Role = u.Role
            }).ToList();
            return Result.Ok(userDtos);
        }
        catch (Exception ex)
        {
            return Result.Fail<List<UserDto>>("Failed to get users" + ex.Message);
        }
    }
}