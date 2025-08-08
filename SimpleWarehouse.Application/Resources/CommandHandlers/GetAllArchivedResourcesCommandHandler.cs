using MediatR;
using SimpleWarehouse.Application.DTOs;
using SimpleWarehouse.Application.Interfaces.Repositories;
using SimpleWarehouse.Application.Resources.Commands;

namespace SimpleWarehouse.Application.Resources.CommandHandlers;

public class GetAllArchivedResourcesCommandHandler(IResourceRepository repository) : IRequestHandler<GetAllArchivedResourcesCommand, Result<IEnumerable<ResourceDto>, ApplicationError>>
{
    public async Task<Result<IEnumerable<ResourceDto>, ApplicationError>> Handle(GetAllArchivedResourcesCommand request,
        CancellationToken cancellationToken)
    {
        var result = await repository.GetAllArchivedAsync(cancellationToken);
        if (result.IsSuccess is false)
            return ResourceErrorResult.Failure<IEnumerable<ResourceDto>>(ApplicationError.UnknownError);
        
        var resources = result.Value!.Select(x => new ResourceDto(x.Id, x.Name));
        return Result<IEnumerable<ResourceDto>, ApplicationError>.Success(resources);
    }
}