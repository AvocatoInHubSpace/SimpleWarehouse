namespace SimpleWarehouse.Application.Interfaces.Repositories;

public interface IUniqueNameRepository<T>
{
    Task<Result<T, RepositoryError>> GetByUniqueNameAsync(string field);
}