using Ardalis.Specification;

namespace eduflowbackend.Core.Abstractions;

public interface IRepository<T> : IRepositoryBase<T> where T : class
{
    bool Exists(Func<T, bool> expression);
}