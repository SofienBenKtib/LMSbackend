using eduflowbackend.Application.Queries;
using eduflowbackend.Core.Abstractions;
using FluentResults;
using FluentResults.Extensions.AspNetCore;
using Mediator;

namespace eduflowbackend.Application.Handlers.User;

public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, string>
{
    private readonly IRepository<Core.User.User> _repository;

    public UpdateUserHandler(IRepository<Core.User.User> repository)
    {
        _repository = repository;
    }

    public async ValueTask<string> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        //  Retrieve the user from the DB
        var user = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (user == null)
        {
            return $"User with ID {request.Id} not found";
        }
        
        //  Updating the user
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Email = request.Email;
        // user.Password=request.Password;
        await _repository.UpdateAsync(user, cancellationToken);
        return $"Updated {user.FirstName} {user.LastName}";
    }
}