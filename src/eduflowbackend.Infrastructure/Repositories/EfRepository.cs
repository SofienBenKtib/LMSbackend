using Ardalis.Specification.EntityFrameworkCore;
using eduflowbackend.Core.Abstractions;
using eduflowbackend.Core.Session;
using eduflowbackend.Core.User;
using Microsoft.EntityFrameworkCore;

namespace eduflowbackend.Infrastructure.Repositories;

public class EfRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T> where T : class
{
    private readonly ApplicationDbContext _appDbContext;

    public EfRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _appDbContext = dbContext;
    }

    public bool Exists(Func<T, bool> expression)
    {
        return _appDbContext.Set<T>().Any(expression);
    }

    public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _appDbContext.Set<T>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<User> FindByIdAsync(Guid requestId)
    {
        return await _appDbContext.Users.FirstOrDefaultAsync(u => u.Id == requestId);
    }

    public async Task<Session> FindSessionByIdAsync(Guid requestId)
    {
        return await _appDbContext.Sessions.FirstOrDefaultAsync(s => s.Id == requestId);
    }
}