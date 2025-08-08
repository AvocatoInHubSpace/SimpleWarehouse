namespace SimpleWarehouse.Application.Interfaces.Repositories;

public interface IRepository<T>
{
    public Task<Result<T, RepositoryError>> GetAsync(Guid id, CancellationToken cancellationToken = default);
    public Task<Result<IEnumerable<T>, RepositoryError>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<Result<RepositoryError>> AddAsync(T entity, CancellationToken cancellationToken = default);
    public Task<Result<RepositoryError>> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    public Task<Result<RepositoryError>> UpdateAsync(T entity, CancellationToken cancellationToken = default);
}

