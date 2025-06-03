using eduflowbackend.Application.Abstractions;
using eduflowbackend.Core.User;
using Microsoft.Extensions.Logging;

namespace eduflowbackend.Infrastructure.Data;

public class DataSeeder : IDataSeeder
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DataSeeder> _logger;

    public DataSeeder(ApplicationDbContext context, ILogger<DataSeeder> logger)
    {
        _context = context;
        _logger = logger;
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
                new User("fname", "surname", "email@email.com", "54791256"),
                new User("another", "user", "A@email.com", "123456"),
                new User("new", "test", "B@email.com", "794613"),
            };
            await _context.Users.AddRangeAsync(users);
            await _context.SaveChangesAsync();
        }
    }
}