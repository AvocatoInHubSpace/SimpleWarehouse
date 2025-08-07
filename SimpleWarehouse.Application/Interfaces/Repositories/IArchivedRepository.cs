using SimpleWarehouse.Core.Domain;

namespace SimpleWarehouse.Application.Interfaces.Repositories;

public interface IArchivedRepository
{
    public Task<Result<RepositoryError>> ArchiveAsync(Guid id);
}