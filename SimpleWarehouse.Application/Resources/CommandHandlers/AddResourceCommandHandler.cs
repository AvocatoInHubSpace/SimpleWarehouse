using MediatR;
using SimpleWarehouse.Application.DTOs;
using SimpleWarehouse.Application.Interfaces.Repositories;
using SimpleWarehouse.Application.Resources.Commands;
using SimpleWarehouse.Core.Domain;

namespace SimpleWarehouse.Application.Resources.CommandHandlers;

public class AddResourceCommandHandler(IResourceRepository repository) : IRequestHandler<AddResourceCommand, Result<IdDto, ApplicationError>>
{
    public async Task<Result<IdDto, ApplicationError>> Handle(AddResourceCommand request,
        CancellationToken cancellationToken)
    {
        var uniqueNameResult = await repository.ValidateUniqueNameAsync(request.Name, cancellationToken);
        if (!uniqueNameResult.IsSuccess)
            return Result<IdDto, ApplicationError>.Failure(uniqueNameResult.Error, uniqueNameResult.ErrorMessage);
        
        var id = Guid.NewGuid();
        var addResult = await repository.AddAsync(new Resource() { Id = id, Name = request.Name },
            cancellationToken);
        return addResult.IsSuccess 
            ? Result<IdDto, ApplicationError>.Success(new IdDto(id)) 
            : ResourceErrorResult.Failure<IdDto>(ApplicationError.UnknownError);
    }
}