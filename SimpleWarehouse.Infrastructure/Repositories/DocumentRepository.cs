using Microsoft.EntityFrameworkCore;
using SimpleWarehouse.Application;
using SimpleWarehouse.Application.Interfaces.Repositories;
using SimpleWarehouse.Core.Domain;
using SimpleWarehouse.Infrastructure.Data;

namespace SimpleWarehouse.Infrastructure.Repositories;

public class DocumentRepository(WarehouseDbContext context) : Repository, IDocumentRepository
{
    public async Task<Result<Document, RepositoryError>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        return await TryFindAsync(async () => await context.Documents.AsNoTracking()
            .Include(d => d.ResourceSupplies)
            .SingleOrDefaultAsync(d => d.Id == id, cancellationToken: cancellationToken));
    }

    public async Task<Result<IEnumerable<Document>, RepositoryError>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await TryGetAsync<IEnumerable<Document>>(async () => await context.Documents.AsNoTracking()
            .Include(d => d.ResourceSupplies)
            .ToListAsync(cancellationToken: cancellationToken));
    }

    public async Task<Result<bool, RepositoryError>> CheckUniqueNameAsync(string number, CancellationToken cancellationToken)
    {
        return await TryGetAsync<bool>(async() =>
        {
            var exists = await context.Documents
                .AsNoTracking()
                .AnyAsync(x => x.Number == number, cancellationToken: cancellationToken);
            return !exists;
        });
    }

    public async Task<Result<RepositoryError>> AddAsync(Document entity, CancellationToken cancellationToken)
    {
        return await TryExecuteAsync(async () =>
        {
            context.Documents.Add(entity);
            await context.SaveChangesAsync(cancellationToken);
        });
    }

    public async Task<Result<RepositoryError>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        return await TryExecuteAsync(async () =>
        {
            await context.Documents.Where(d => d.Id == id).ExecuteDeleteAsync(cancellationToken: cancellationToken);
        });
    }

    public async Task<Result<RepositoryError>> UpdateAsync(Document entity, CancellationToken cancellationToken)
    {
        return await TryExecuteAsync(async () =>
        {
            context.Documents.Update(entity);
            await context.SaveChangesAsync(cancellationToken);
        });
    }
}