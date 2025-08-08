namespace SimpleWarehouse.Application.Interfaces.Repositories;

public interface IEntityUsageRepository<T>
{
    Task<Result<bool, RepositoryError>> IsInUseAsync(Guid id, CancellationToken cancellationToken = default);
}