namespace SimpleWarehouse.Application.Interfaces.Repositories;

public interface IUniqueNameRepository
{
    Task<Result<bool, RepositoryError>> CheckUniqueNameAsync(string name, CancellationToken cancellationToken = default);
}