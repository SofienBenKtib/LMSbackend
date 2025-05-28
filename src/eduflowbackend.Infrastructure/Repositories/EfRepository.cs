using Ardalis.Specification.EntityFrameworkCore;
using eduflowbackend.Core.Abstractions;

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
}