using Microsoft.EntityFrameworkCore;
using SimpleWarehouse.Application;
using SimpleWarehouse.Application.Interfaces.Repositories;
using SimpleWarehouse.Core.Domain;
using SimpleWarehouse.Infrastructure.Data;

namespace SimpleWarehouse.Infrastructure.Repositories;

public class DocumentRepository(WarehouseDbContext context) : Repository, IDocumentRepository
{
    public async Task<Result<Document, RepositoryError>> GetAsync(Guid id)
    {
        return await TryFindAsync(async () => await context.Documents.AsNoTracking()
            .Include(d => d.ResourceSupplies)
            .SingleOrDefaultAsync(d => d.Id == id));
    }

    public async Task<Result<IEnumerable<Document>, RepositoryError>> GetAllAsync()
    {
        return await TryGetAsync<IEnumerable<Document>>(async () => await context.Documents.AsNoTracking()
            .Include(d => d.ResourceSupplies)
            .ToListAsync());
    }

    public async Task<Result<Document, RepositoryError>> GetByUniqueNameAsync(string number)
    {
        return await TryFindAsync(async () => await context.Documents.AsNoTracking()
            .SingleOrDefaultAsync(d => d.Number == number));
    }

    public async Task<Result<RepositoryError>> AddAsync(Document entity)
    {
        return await TryExecuteAsync(async () =>
        {
            context.Documents.Add(entity);
            await context.SaveChangesAsync();
        });
    }

    public async Task<Result<RepositoryError>> DeleteAsync(Guid id)
    {
        return await TryExecuteAsync(async () =>
        {
            await context.Documents.Where(d => d.Id == id).ExecuteDeleteAsync();
        });
    }

    public async Task<Result<RepositoryError>> UpdateAsync(Document entity)
    {
        return await TryExecuteAsync(async () =>
        {
            context.Documents.Update(entity);
            await context.SaveChangesAsync();
        });
    }
}