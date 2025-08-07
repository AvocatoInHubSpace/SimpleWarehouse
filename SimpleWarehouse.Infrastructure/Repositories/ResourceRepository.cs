using Microsoft.EntityFrameworkCore;
using SimpleWarehouse.Application;
using SimpleWarehouse.Application.Interfaces.Repositories;
using SimpleWarehouse.Core.Domain;
using SimpleWarehouse.Infrastructure.Data;
using SimpleWarehouse.Infrastructure.Extanstions;

namespace SimpleWarehouse.Infrastructure.Repositories;

public class ResourceRepository(WarehouseDbContext context) : Repository, IResourceRepository
{
    public async Task<Result<Resource, RepositoryError>> GetAsync(Guid id)
    {
        return await TryFindAsync(async() => await context.Resources.AsNoTracking()
            .NotArchived()
            .FirstOrDefaultAsync(d => d.Id == id));
    }

    public async Task<Result<Resource, RepositoryError>> GetByUniqueNameAsync(string field)
    {
        return await TryFindAsync(async() =>
        {
            return await context.Resources.AsNoTracking()
                .Where(x => x.Name == field)
                .FirstOrDefaultAsync();
        });
    }

    public async Task<Result<RepositoryError>> AddAsync(Resource entity)
    {
        return await TryExecuteAsync(async () =>
        {
            context.Resources.Add(entity);
            await context.SaveChangesAsync();
        });
    }

    public async Task<Result<RepositoryError>> DeleteAsync(Guid id)
    {
        return await TryExecuteAsync(async () =>
        {
            await context.Resources.Where(d => d.Id == id).ExecuteDeleteAsync();
        });
    }

    public async Task<Result<RepositoryError>> UpdateAsync(Resource entity)
    {
        return await TryExecuteAsync(async () =>
        {
            context.Resources.Update(entity);
            await context.SaveChangesAsync();
        });
    }

    public async Task<Result<RepositoryError>> ArchiveAsync(Guid id)
    {
        return await TryExecuteAsync(async () =>
        {
            await context.Resources.Where(x => x.Id == id)
                .ExecuteUpdateAsync(x => x.SetProperty(m => m.IsArchived, true));
        });
    }

    public async Task<Result<bool, RepositoryError>> IsInUseAsync(Guid id)
    {
        return await TryGetAsync(async ()
            => await context.ResourceSupplies.Where(x => x.ResourceId == id).AnyAsync());
    }
}