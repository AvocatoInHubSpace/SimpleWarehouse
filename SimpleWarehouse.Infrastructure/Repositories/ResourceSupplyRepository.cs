using Microsoft.EntityFrameworkCore;
using SimpleWarehouse.Application;
using SimpleWarehouse.Application.Interfaces.Repositories;
using SimpleWarehouse.Core.Domain;
using SimpleWarehouse.Infrastructure.Data;

namespace SimpleWarehouse.Infrastructure.Repositories;

public class ResourceSupplyRepository(WarehouseDbContext context) : Repository, IResourceSupplyRepository
{
    public async Task<Result<ResourceSupply, RepositoryError>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        return await TryFindAsync(async() => await context.ResourceSupplies.AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken: cancellationToken));
    }

    public async Task<Result<IEnumerable<ResourceSupply>, RepositoryError>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await TryFindAsync<IEnumerable<ResourceSupply>>(async() => await context.ResourceSupplies.AsNoTracking()
            .ToListAsync(cancellationToken: cancellationToken));
    }

    public async Task<Result<RepositoryError>> AddAsync(ResourceSupply entity, CancellationToken cancellationToken)
    {
        return await TryExecuteAsync(async () =>
        {
            context.ResourceSupplies.Add(entity);
            await context.SaveChangesAsync(cancellationToken);
        });
    }

    public async Task<Result<RepositoryError>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        return await TryExecuteAsync(async () =>
        {
            await context.ResourceSupplies.Where(d => d.Id == id).ExecuteDeleteAsync(cancellationToken: cancellationToken);
        });
    }

    public async Task<Result<RepositoryError>> UpdateAsync(ResourceSupply entity, CancellationToken cancellationToken)
    {
        return await TryExecuteAsync(async () =>
        {
            context.ResourceSupplies.Update(entity);
            await context.SaveChangesAsync(cancellationToken);
        });
    }
}