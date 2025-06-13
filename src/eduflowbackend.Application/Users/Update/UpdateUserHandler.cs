using eduflowbackend.Application.Queries;
using eduflowbackend.Core.Abstractions;
using FluentResults;
using FluentResults.Extensions.AspNetCore;
using Mediator;

namespace eduflowbackend.Application.Handlers.User;

public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, Result<string>>
{
    private readonly IRepository<Core.User.User> _repository;

    public UpdateUserHandler(IRepository<Core.User.User> repository)
    {
        _repository = repository;
    }

    public async ValueTask<Result<string>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        //  Retrieve the user from the DB
        var user = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (user == null)
        {
            return Result.Fail("User not found");
        }

        //  Updating the user
        user.Update(request.FirstName, request.LastName,request.Email, request.PhoneNumber);
        // user.Password=request.Password;
        await _repository.UpdateAsync(user, cancellationToken);
        return Result.Ok("User updated");
    }
}