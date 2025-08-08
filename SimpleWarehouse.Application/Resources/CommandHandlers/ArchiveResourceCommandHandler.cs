using MediatR;
using SimpleWarehouse.Application.DTOs;
using SimpleWarehouse.Application.Interfaces.Repositories;
using SimpleWarehouse.Application.Resources.Commands;

namespace SimpleWarehouse.Application.Resources.CommandHandlers;

public class ArchiveResourceCommandHandler(IResourceRepository repository) : IRequestHandler<ArchiveResourceCommand, Result<IdDto, ApplicationError>>
{
    public async Task<Result<IdDto, ApplicationError>> Handle(ArchiveResourceCommand request, CancellationToken cancellationToken)
    {
        var getResult = await repository.GetIfExistsAsync(request.Id, cancellationToken);
        if (!getResult.IsSuccess)
            return ResourceErrorResult.Failure<IdDto>(getResult.Error);
        
        var resource = getResult.Value!;
        if (resource.IsArchived is false)
        {
            var result = await repository.ArchiveAsync(request.Id, cancellationToken);
            if (result.IsSuccess is false) 
                return ResourceErrorResult.Failure<IdDto>(ApplicationError.UnknownError);
        }
        
        return Result<IdDto, ApplicationError>.Success(new IdDto(resource.Id));
    }
}