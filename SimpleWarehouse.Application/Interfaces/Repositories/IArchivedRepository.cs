using SimpleWarehouse.Core.Domain;

namespace SimpleWarehouse.Application.Interfaces.Repositories;

public interface IArchivedRepository<T> where T : ArchivedEntity
{
    public Task<Result<RepositoryError>> ArchiveAsync(Guid id);
    
    public Task<Result<IEnumerable<T>, RepositoryError>> GetAllArchivedAsync();
}