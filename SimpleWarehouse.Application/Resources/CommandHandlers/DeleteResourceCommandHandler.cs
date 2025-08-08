using MediatR;
using SimpleWarehouse.Application.DTOs;
using SimpleWarehouse.Application.Interfaces.Repositories;
using SimpleWarehouse.Application.Resources.Commands;

namespace SimpleWarehouse.Application.Resources.CommandHandlers;

public class DeleteResourceCommandHandler(IResourceRepository repository) : IRequestHandler<DeleteResourceCommand, Result<IdDto, ApplicationError>>
{
    public async Task<Result<IdDto, ApplicationError>> Handle(DeleteResourceCommand request, CancellationToken cancellationToken)
    {
        var getResult = await repository.GetIfExistsAsync(request.Id, cancellationToken);
        if (getResult.IsSuccess is false)
            return ResourceErrorResult.Failure<IdDto>(getResult.Error);
        
        var inUseResult = await repository.IsInUseAsync(request.Id, cancellationToken);
        if (inUseResult.IsSuccess is false)
            return ResourceErrorResult.Failure<IdDto>(ApplicationError.UnknownError);
        
        if (inUseResult.Value is true)
            return ResourceErrorResult.Failure<IdDto>(ApplicationError.DeleteNotAllowedResourceInUse);
        
        var resource = getResult.Value!;
        var result = await repository.DeleteAsync(request.Id, cancellationToken);
        return result.IsSuccess is true
            ? Result<IdDto, ApplicationError>.Success(new IdDto(resource.Id))
            : ResourceErrorResult.Failure<IdDto>(ApplicationError.UnknownError);
    }
}