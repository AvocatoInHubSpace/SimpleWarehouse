using Microsoft.EntityFrameworkCore;
using SimpleWarehouse.Application;
using SimpleWarehouse.Application.Interfaces.Repositories;
using SimpleWarehouse.Core.Domain;
using SimpleWarehouse.Infrastructure.Data;
using SimpleWarehouse.Infrastructure.Extanstions;

namespace SimpleWarehouse.Infrastructure.Repositories;

public class MeasureUnitRepository(WarehouseDbContext context) : Repository, IMeasureUnitRepository
{
    public async Task<Result<MeasureUnit, RepositoryError>> GetAsync(Guid id)
    {
        return await TryFindAsync(async() => await context.MeasureUnits.AsNoTracking()
            .NotArchived()
            .FirstOrDefaultAsync(d => d.Id == id));
    }

    public async Task<Result<IEnumerable<MeasureUnit>, RepositoryError>> GetAllAsync()
    {
        return await TryGetAsync<IEnumerable<MeasureUnit>>(async() => await context.MeasureUnits.AsNoTracking()
            .NotArchived()
            .ToListAsync());
    }

    public async Task<Result<IEnumerable<MeasureUnit>, RepositoryError>> GetAllArchivedAsync()
    {
        return await TryGetAsync<IEnumerable<MeasureUnit>>(async() => await context.MeasureUnits.AsNoTracking()
            .Archived()
            .ToListAsync());
    }

    public async Task<Result<MeasureUnit, RepositoryError>> GetByUniqueNameAsync(string name)
    {
        return await TryFindAsync(async() => await context.MeasureUnits
            .AsNoTracking()
            .Where(x => x.Name == name)
            .FirstOrDefaultAsync());
    }

    public async Task<Result<RepositoryError>> AddAsync(MeasureUnit entity)
    {
        return await TryExecuteAsync(async () =>
        {
            context.MeasureUnits.Add(entity);
            await context.SaveChangesAsync();
        });
    }

    public async Task<Result<RepositoryError>> DeleteAsync(Guid id)
    {
        return await TryExecuteAsync(async () =>
        {
            await context.MeasureUnits
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();
        });
    }

    public async Task<Result<RepositoryError>> UpdateAsync(MeasureUnit entity)
    {
        return await TryExecuteAsync(async () =>
        {
            context.MeasureUnits.Update(entity);
            await context.SaveChangesAsync();
        });
    }

    public async Task<Result<bool, RepositoryError>> IsInUseAsync(Guid id)
    {
        return await TryGetAsync(async ()
            => await context.ResourceSupplies.Where(x => x.MeasureUnitId == id).AnyAsync());
    }


    public async Task<Result<RepositoryError>> ArchiveAsync(Guid id)
    {
        return await TryExecuteAsync(async () =>
        {
            await context.MeasureUnits
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(x => x.SetProperty(m => m.IsArchived, true));
        });
    }
}