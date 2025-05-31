using eduflowbackend.Application.Abstractions;
using eduflowbackend.Core.Exceptions;
using eduflowbackend.Core.User;
using Mediator;

namespace eduflowbackend.Application.Users.Get;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, User>
{
    private readonly IApplicationDbContext _context;

    public GetUserByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<User> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FindAsync(request.Id);
        if (user == null)
        {
            throw new FileNotFoundException(nameof(user), request.Id.ToString()); 
            // throw new NotFoundError(nameof(user), "Does not exist");
        }

        return user;
    }
}