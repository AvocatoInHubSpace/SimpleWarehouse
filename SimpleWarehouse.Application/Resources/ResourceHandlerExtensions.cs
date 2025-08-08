using SimpleWarehouse.Application.Interfaces.Repositories;
using SimpleWarehouse.Core.Domain;

namespace SimpleWarehouse.Application.Resources;

public static class ResourceHandlerExtensions
{
    public static async Task<Result<Resource, ApplicationError>> GetIfExistsAsync(
        this IResourceRepository repository, Guid id, CancellationToken cancellationToken)
    {
        var getResult = await repository.GetAsync(id, cancellationToken);
        
        var result = getResult switch
        {
            {IsSuccess: true} => Result<Resource, ApplicationError>.Success(getResult.Value!),
            {IsSuccess: false, Error: RepositoryError.NotFound}  => ResourceErrorResult.Failure<Resource>(ApplicationError.NotFound),
            _ => ResourceErrorResult.Failure<Resource>(ApplicationError.UnknownError)
        };
        return result;
    }
    
    public static async Task<Result<ApplicationError>> ValidateUniqueNameAsync(
        this IResourceRepository repository, string name, CancellationToken cancellationToken = default)
    {
        var uniqueNameResult = await repository.CheckUniqueNameAsync(name, cancellationToken);

        var result = uniqueNameResult switch
        {
            {IsSuccess: true, Value: true} => Result<ApplicationError>.Success(),
            {IsSuccess: true, Value: false}  => ResourceErrorResult.Failure(ApplicationError.DuplicateValue),
            _ => ResourceErrorResult.Failure(ApplicationError.UnknownError)
        };
        return result;
    }
}