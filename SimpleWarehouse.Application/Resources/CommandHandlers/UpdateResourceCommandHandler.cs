using MediatR;
using SimpleWarehouse.Application.DTOs;
using SimpleWarehouse.Application.Interfaces.Repositories;
using SimpleWarehouse.Application.Resources.Commands;
using SimpleWarehouse.Core.Domain;

namespace SimpleWarehouse.Application.Resources.CommandHandlers;

public class UpdateResourceCommandHandler(IResourceRepository repository) : IRequestHandler<UpdateResourceCommand, Result<IdDto, ApplicationError>>
{
    public async Task<Result<IdDto, ApplicationError>> Handle(UpdateResourceCommand request, CancellationToken cancellationToken)
    {
        var getResult = await repository.GetIfExistsAsync(request.Resource.Id, cancellationToken);
        if (getResult.IsSuccess is false)
            return ResourceErrorResult.Failure<IdDto>(getResult.Error);

        var newResource = request.Resource;
        var oldResource = getResult.Value!;
        if (oldResource.Name != newResource.Name)
        {
            var uniqueNameResult = await repository.ValidateUniqueNameAsync(newResource.Name, cancellationToken);
            if (uniqueNameResult.IsSuccess is false) 
                return ResourceErrorResult.Failure<IdDto>(uniqueNameResult.Error);
        }
        
        var updateResource = new Resource
            { Id = newResource.Id, Name = newResource.Name, IsArchived = oldResource.IsArchived };
        
        var result = await repository.UpdateAsync(updateResource, cancellationToken);
        return result.IsSuccess 
            ? Result<IdDto, ApplicationError>.Success(new IdDto(newResource.Id)) 
            : ResourceErrorResult.Failure<IdDto>(ApplicationError.UnknownError);
    }
}