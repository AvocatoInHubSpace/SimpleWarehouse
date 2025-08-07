using Microsoft.EntityFrameworkCore;
using SimpleWarehouse.Application;
using SimpleWarehouse.Application.Interfaces.Repositories;
using SimpleWarehouse.Core.Domain;
using SimpleWarehouse.Infrastructure.Data;

namespace SimpleWarehouse.Infrastructure.Repositories;

public class ResourceSupplyRepository(WarehouseDbContext context) : Repository, IResourceSupplyRepository
{
    public async Task<Result<ResourceSupply, RepositoryError>> GetAsync(Guid id)
    {
        return await TryFindAsync(async() => await context.ResourceSupplies.AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == id));
    }

    public async Task<Result<IEnumerable<ResourceSupply>, RepositoryError>> GetAllAsync()
    {
        return await TryFindAsync<IEnumerable<ResourceSupply>>(async() => await context.ResourceSupplies.AsNoTracking()
            .ToListAsync());
    }

    public async Task<Result<RepositoryError>> AddAsync(ResourceSupply entity)
    {
        return await TryExecuteAsync(async () =>
        {
            context.ResourceSupplies.Add(entity);
            await context.SaveChangesAsync();
        });
    }

    public async Task<Result<RepositoryError>> DeleteAsync(Guid id)
    {
        return await TryExecuteAsync(async () =>
        {
            await context.ResourceSupplies.Where(d => d.Id == id).ExecuteDeleteAsync();
        });
    }

    public async Task<Result<RepositoryError>> UpdateAsync(ResourceSupply entity)
    {
        return await TryExecuteAsync(async () =>
        {
            context.ResourceSupplies.Update(entity);
            await context.SaveChangesAsync();
        });
    }
}