using eduflowbackend.Application.Abstractions;
using eduflowbackend.Application.Users.Create;
using eduflowbackend.Core.User;
using Mediator;
using Microsoft.Extensions.Logging;

namespace eduflowbackend.Infrastructure.Data;

public class DataSeeder : IDataSeeder
{
    private readonly IMediator _mediator;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DataSeeder> _logger;

    public DataSeeder(ApplicationDbContext context, ILogger<DataSeeder> logger, IMediator mediator)
    {
        _context = context;
        _logger = logger;
        _mediator = mediator;
    }

    public async Task SeedAsync()
    {
        try
        {
            await _context.Database.EnsureCreatedAsync();
            await SeedUsersAsync();
            //  Sessions & Resources methods TBA
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }

    private async Task SeedUsersAsync()
    {
        if (!_context.Users.Any())
        {
            _logger.LogInformation("Seeding users...");
            var users = new List<User>
            {
                new("fname", "surname", "email@email.com", "54791256"),
                new("another", "user", "A@email.com", "123456"),
                new("new", "test", "B@email.com", "794613"),
            };
            foreach (var command in users.Select(user => new CreateUserCommand(user.FirstName, user.LastName, user.Email, user.PhoneNumber, user.Role)))
            {
                await _mediator.Send(command);
            }
        }
    }
}