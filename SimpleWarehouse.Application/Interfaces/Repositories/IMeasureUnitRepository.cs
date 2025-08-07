using SimpleWarehouse.Core.Domain;

namespace SimpleWarehouse.Application.Interfaces.Repositories;

public interface IMeasureUnitRepository : IArchivedRepository<MeasureUnit>, IRepository<MeasureUnit>, IUniqueNameRepository<MeasureUnit>,
    IEntityUsageRepository<Resource>
{

}