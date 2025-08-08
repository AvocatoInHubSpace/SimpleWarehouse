using MediatR;
using SimpleWarehouse.Application.DTOs;
using SimpleWarehouse.Application.Interfaces.Repositories;
using SimpleWarehouse.Application.Resources.Commands;

namespace SimpleWarehouse.Application.Resources.CommandHandlers;

public class GetResourceCommandHandler(IResourceRepository repository) : IRequestHandler<GetResourceCommand, Result<ResourceDto, ApplicationError>>
{
    public async Task<Result<ResourceDto, ApplicationError>> Handle(GetResourceCommand request, CancellationToken cancellationToken)
    {
        var result = await repository.GetIfExistsAsync(request.Id, cancellationToken);
        if (!result.IsSuccess)
            return ResourceErrorResult.Failure<ResourceDto>(result.Error);
        
        var resource = new ResourceDto(result.Value!.Id, result.Value!.Name);
        return Result<ResourceDto, ApplicationError>.Success(resource);
    }
}