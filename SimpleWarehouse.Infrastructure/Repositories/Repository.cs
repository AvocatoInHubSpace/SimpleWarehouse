using Microsoft.EntityFrameworkCore;
using SimpleWarehouse.Application;

namespace SimpleWarehouse.Infrastructure.Repositories;

public abstract class Repository
{
    protected async Task<Result<T, RepositoryError>> TryFindAsync<T>(Func<Task<T?>> action) where T : class
    {
        try
        {
            var result = await action();
            return result is not null
                ? Result<T, RepositoryError>.Success(result)
                : Result<T, RepositoryError>.Failure(RepositoryError.NotFound);
        }
        catch (DbUpdateException)
        {
            return Result<T, RepositoryError>.Failure(RepositoryError.InternalError);
        }
    }
    protected async Task<Result<T, RepositoryError>> TryGetAsync<T>(Func<Task<T>> action) where T : notnull
    {
        try
        {
            var result = await action();
            return Result<T, RepositoryError>.Success(result);
        }
        catch (DbUpdateException)
        {
            return Result<T, RepositoryError>.Failure(RepositoryError.InternalError);
        }
    }

    protected async Task<Result<RepositoryError>> TryExecuteAsync(Func<Task> action)
    {
        try
        {
            await action();
            return Result<RepositoryError>.Success();
        }
        catch (DbUpdateException)
        {
            return Result<RepositoryError>.Failure(RepositoryError.InternalError);
        }
    }
}