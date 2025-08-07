namespace SimpleWarehouse.Application.Interfaces.Repositories;

public interface IRepository<T>
{
    public Task<Result<T, RepositoryError>> GetAsync(Guid id);
    public Task<Result<RepositoryError>> AddAsync(T entity);
    public Task<Result<RepositoryError>> DeleteAsync(Guid id);
    public Task<Result<RepositoryError>> UpdateAsync(T entity);
}

