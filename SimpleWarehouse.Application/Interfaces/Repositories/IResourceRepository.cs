using SimpleWarehouse.Core.Domain;

namespace SimpleWarehouse.Application.Interfaces.Repositories;

public interface IResourceRepository : IArchivedRepository<Resource>, IRepository<Resource>, IUniqueNameRepository<Resource>,
    IEntityUsageRepository<Resource>
{
}