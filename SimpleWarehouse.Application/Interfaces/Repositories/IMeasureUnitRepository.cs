using SimpleWarehouse.Core.Domain;

namespace SimpleWarehouse.Application.Interfaces.Repositories;

public interface IMeasureUnitRepository : IArchivedRepository, IRepository<MeasureUnit>, IUniqueNameRepository<MeasureUnit>,
    IEntityUsageRepository<Resource>
{

}