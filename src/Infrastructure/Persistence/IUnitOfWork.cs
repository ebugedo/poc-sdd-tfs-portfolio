using System.Threading;

namespace Portfolio.Infrastructure.Persistence;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}