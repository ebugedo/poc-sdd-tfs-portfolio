using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Portfolio.Infrastructure.Persistence;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(object id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<T>> ListAsync(CancellationToken cancellationToken = default);
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
}