using eduflowbackend.Application.Abstractions;
using eduflowbackend.Core.Abstractions;
using eduflowbackend.Core.Exceptions;
using eduflowbackend.Core.User;
using FluentResults;
using Mediator;

namespace eduflowbackend.Application.Users.Create;

public class CreateUserHandler : IRequestHandler<CreateUserCommand, Result<Guid>>
{
    private readonly IRepository<User> _repository;
    private readonly IIdentityProviderService _identityProviderService;
    private readonly IEmailService _emailService;

    public CreateUserHandler(IRepository<User> repository, IIdentityProviderService identityProviderService,
        IEmailService emailService)
    {
        _repository = repository;
        _identityProviderService = identityProviderService;
        _emailService = emailService;
    }

    public async ValueTask<Result<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        //  Validating user uniqueness
        var username = User.GetUsername(request.Firstname, request.Lastname);
        if (_repository.Exists(user => user.GetUsername() == username))
        {
            return Result.Fail(new ConflictError("User", "Firstname and lastname",
                $"{request.Firstname} {request.Lastname}"));
        }

        if (_repository.Exists(user => user.Email == request.Email))
        {
            return Result.Fail(new ConflictError("User", "Email", request.Email));
        }

        //  Creating the user in the Identity Provider (Keycloak)
        var identityProviderResult = await _identityProviderService.CreateUserAsync(request);

        if (identityProviderResult.IsFailed)
        {
            return identityProviderResult.ToResult();
        }

        //  Assigning the role to the user
        var userId = identityProviderResult.Value.userId;
        var roleAssignmentResult = await _identityProviderService.AssignUserToRoleAsync(
            userId,
            Enum.GetName(request.Role));

        if (roleAssignmentResult.IsFailed)
        {
            // Compensate
            await _identityProviderService.DeleteUserAsync(userId);
            return Result.Fail(roleAssignmentResult.Errors.First());
        }

        var user = new User(request.Firstname, request.Lastname, request.Email, request.PhoneNumber);

        user.UpdateIdentityGuid(userId);

        await _repository.AddAsync(user, cancellationToken);

        var emailBody = $"""
                         Dear {username},

                         Welcome to EduFlow! We’re excited to have you on board.

                         Here are your account details:
                         	•	Username: {user.Email}
                         	•	Temporary Password: {identityProviderResult.Value.temporaryPassword}

                         For security reasons, please log in and change your password immediately by following these steps:
                         	1.	Go to [Login Page URL].
                         	2.	Enter your username and temporary password.
                         	3.	Follow the prompts to set a new password.

                         If you have any questions or need assistance, feel free to contact our support team at [Support Email].

                         Best regards,
                         Eduflow admin
                         """;

        await _emailService.Send(user.Email, "Welcome to Eduflow", emailBody);

        return user.Id;
    }
}