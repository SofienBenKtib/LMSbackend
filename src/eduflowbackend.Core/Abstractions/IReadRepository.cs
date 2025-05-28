using Ardalis.Specification;

namespace eduflowbackend.Core.Abstractions;

public interface IReadRepository<T> : IReadRepositoryBase<T> where T: class
{
    bool Exists(Func<T, bool> expression);
}