using SimpleWarehouse.Core.Domain;

namespace SimpleWarehouse.Application.Interfaces.Repositories;

public interface IResourceRepository : IArchivedRepository, IRepository<Resource>, IUniqueNameRepository<Resource>,
    IEntityUsageRepository<Resource>
{
}