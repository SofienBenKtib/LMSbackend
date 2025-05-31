using eduflowbackend.Application.Abstractions;
using eduflowbackend.Core.Abstractions;
using eduflowbackend.Core.User;
using FluentResults;
using Mediator;

namespace eduflowbackend.Application.Queries;

public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, Result<Guid>>
{
    private readonly IRepository<User> _repository;
    private readonly IIdentityProviderService _identityProviderService;

    public DeleteUserHandler(IRepository<User> repository, IIdentityProviderService identityProviderService)
    {
        _repository = repository;
        _identityProviderService = identityProviderService;
    }

    public async ValueTask<Result<Guid>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        //  Finding the user in the database
        var user = await _repository.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
        {
            return Result.Fail<Guid>("User not found");
        }

        //  Deleting from Keycloak (if exists)
        if (!string.IsNullOrEmpty(user.Id.ToString()))
        {
            var identityResult = await _identityProviderService.DeleteUserAsync(user.Id.ToString());
            if (identityResult.IsFailed)
            {
                return identityResult;
            }
        }

        //  Delete fromt the database
        await _repository.DeleteAsync(user);
        return Result.Ok();
    }
}