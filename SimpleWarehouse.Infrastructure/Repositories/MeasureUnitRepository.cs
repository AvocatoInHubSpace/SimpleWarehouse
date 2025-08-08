using Microsoft.EntityFrameworkCore;
using SimpleWarehouse.Application;
using SimpleWarehouse.Application.Interfaces.Repositories;
using SimpleWarehouse.Core.Domain;
using SimpleWarehouse.Infrastructure.Data;
using SimpleWarehouse.Infrastructure.Extanstions;

namespace SimpleWarehouse.Infrastructure.Repositories;

public class MeasureUnitRepository(WarehouseDbContext context) : Repository, IMeasureUnitRepository
{
    public async Task<Result<MeasureUnit, RepositoryError>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        return await TryFindAsync(async() => await context.MeasureUnits.AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken: cancellationToken));
    }

    public async Task<Result<IEnumerable<MeasureUnit>, RepositoryError>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await TryGetAsync<IEnumerable<MeasureUnit>>(async() => await context.MeasureUnits.AsNoTracking()
            .NotArchived()
            .ToListAsync(cancellationToken: cancellationToken));
    }

    public async Task<Result<IEnumerable<MeasureUnit>, RepositoryError>> GetAllArchivedAsync(CancellationToken cancellationToken)
    {
        return await TryGetAsync<IEnumerable<MeasureUnit>>(async() => await context.MeasureUnits.AsNoTracking()
            .Archived()
            .ToListAsync(cancellationToken: cancellationToken));
    }

    public async Task<Result<bool, RepositoryError>> CheckUniqueNameAsync(string name, CancellationToken cancellationToken)
    {
        return await TryGetAsync<bool>(async() =>
        {
            var exists = await context.MeasureUnits
                .AsNoTracking()
                .AnyAsync(x => x.Name == name,
                    cancellationToken: cancellationToken);
            return !exists;
        });
    }

    public async Task<Result<RepositoryError>> AddAsync(MeasureUnit entity, CancellationToken cancellationToken)
    {
        return await TryExecuteAsync(async () =>
        {
            context.MeasureUnits.Add(entity);
            await context.SaveChangesAsync(cancellationToken);
        });
    }

    public async Task<Result<RepositoryError>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        return await TryExecuteAsync(async () =>
        {
            await context.MeasureUnits
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync(cancellationToken: cancellationToken);
        });
    }

    public async Task<Result<RepositoryError>> UpdateAsync(MeasureUnit entity, CancellationToken cancellationToken)
    {
        return await TryExecuteAsync(async () =>
        {
            context.MeasureUnits.Update(entity);
            await context.SaveChangesAsync(cancellationToken);
        });
    }

    public async Task<Result<bool, RepositoryError>> IsInUseAsync(Guid id, CancellationToken cancellationToken)
    {
        return await TryGetAsync(async ()
            => await context.ResourceSupplies.Where(x => x.MeasureUnitId == id)
                .AnyAsync(cancellationToken: cancellationToken));
    }


    public async Task<Result<RepositoryError>> ArchiveAsync(Guid id, CancellationToken cancellationToken)
    {
        return await TryExecuteAsync(async () =>
        {
            await context.MeasureUnits
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(x 
                    => x.SetProperty(m => m.IsArchived, true), cancellationToken: cancellationToken);
        });
    }
}