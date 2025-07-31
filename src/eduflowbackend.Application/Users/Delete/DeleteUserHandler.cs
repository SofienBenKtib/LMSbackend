using eduflowbackend.Application.Abstractions;
using eduflowbackend.Core.Abstractions;
using eduflowbackend.Core.Exceptions;
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
        try
        {
            //  Finding the user in the database
            var user = await _repository.GetByIdAsync(request.UserId, cancellationToken);
            if (user == null)
            {
                return Result.Fail(new NotFoundError("User", request.UserId.ToString()));
            }

            //  Deleting from Keycloak (if exists)
            // if (!string.IsNullOrEmpty(user.Id.ToString()))
            if (user.Id != Guid.Empty)
            {
                var identityResult = await _identityProviderService.DeleteUserAsync(user.Id.ToString());
                if (identityResult.IsFailed)
                {
                    //return identityResult;
                    if (identityResult.HasError<NotFoundError>())
                    {
                        Console.WriteLine($"Keycloak user {user.Id} not found - proceeding with DB deletion");
                    }
                    else
                    {
                        return identityResult.ToResult<Guid>();
                    }
                }
            }

            //  Delete from the database
            await _repository.DeleteAsync(user);
            return Result.Ok(request.UserId);
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error("Failed to delete user").CausedBy(ex));
        }
    }
}