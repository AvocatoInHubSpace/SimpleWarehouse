using Microsoft.EntityFrameworkCore;
using SimpleWarehouse.Application;
using SimpleWarehouse.Application.Interfaces.Repositories;
using SimpleWarehouse.Core.Domain;
using SimpleWarehouse.Infrastructure.Data;
using SimpleWarehouse.Infrastructure.Extanstions;

namespace SimpleWarehouse.Infrastructure.Repositories;

public class ResourceRepository(WarehouseDbContext context) : Repository, IResourceRepository
{
    public async Task<Result<Resource, RepositoryError>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        return await TryFindAsync(async() => await context.Resources.AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken: cancellationToken));
    }

    public async Task<Result<IEnumerable<Resource>, RepositoryError>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await TryGetAsync<IEnumerable<Resource>>(async() => await context.Resources.AsNoTracking()
            .NotArchived()
            .ToListAsync(cancellationToken: cancellationToken));
    }

    public async Task<Result<IEnumerable<Resource>, RepositoryError>> GetAllArchivedAsync(CancellationToken cancellationToken)
    {
        return await TryGetAsync<IEnumerable<Resource>>(async() => await context.Resources.AsNoTracking()
            .Archived()
            .ToListAsync(cancellationToken: cancellationToken));
    }

    public async Task<Result<bool, RepositoryError>> CheckUniqueNameAsync(string name, CancellationToken cancellationToken)
    {
        return await TryGetAsync<bool>(async() =>
        {
            var exists = await context.Resources
                .AsNoTracking()
                .AnyAsync(x => x.Name == name, cancellationToken: cancellationToken);
            return !exists;
        });
    }

    public async Task<Result<RepositoryError>> AddAsync(Resource entity, CancellationToken cancellationToken)
    {
        return await TryExecuteAsync(async () =>
        {
            context.Resources.Add(entity);
            await context.SaveChangesAsync(cancellationToken);
        });
    }

    public async Task<Result<RepositoryError>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        return await TryExecuteAsync(async () =>
        {
            await context.Resources.Where(d => d.Id == id).ExecuteDeleteAsync(cancellationToken: cancellationToken);
        });
    }

    public async Task<Result<RepositoryError>> UpdateAsync(Resource entity, CancellationToken cancellationToken)
    {
        return await TryExecuteAsync(async () =>
        {
            context.Resources.Update(entity);
            await context.SaveChangesAsync(cancellationToken);
        });
    }

    public async Task<Result<RepositoryError>> ArchiveAsync(Guid id, CancellationToken cancellationToken)
    {
        return await TryExecuteAsync(async () =>
        {
            await context.Resources.Where(x => x.Id == id)
                .ExecuteUpdateAsync(x 
                    => x.SetProperty(m => m.IsArchived, true), cancellationToken: cancellationToken);
        });
    }

    public async Task<Result<bool, RepositoryError>> IsInUseAsync(Guid id, CancellationToken cancellationToken)
    {
        return await TryGetAsync(async ()
            => await context.ResourceSupplies
                .Where(x => x.ResourceId == id)
                .AnyAsync(cancellationToken: cancellationToken));
    }
}