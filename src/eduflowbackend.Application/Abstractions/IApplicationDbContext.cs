using eduflowbackend.Core.User;
using Microsoft.EntityFrameworkCore;
namespace eduflowbackend.Application.Abstractions;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}