using eduflowbackend.Application.dtos;
using eduflowbackend.Core.Abstractions;
using eduflowbackend.Core.Exceptions;
using eduflowbackend.Core.User;
using FluentResults;
using Mediator;

namespace eduflowbackend.Application.Users.Get;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserDTO>>
{
    private readonly IRepository<UserDTO> _repository;

    public GetUserByIdQueryHandler(IRepository<UserDTO> repository)
    {
        _repository = repository;
    }

    public async ValueTask<Result<UserDTO>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _repository.FindByIdAsync(request.Id);
        if (user == null)
        {
            return Result.Fail<UserDTO>(new NotFoundError(nameof(User), request.Id.ToString()));
        }

        var userDto = new UserDTO
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
        };
        return Result.Ok(userDto);
    }
}