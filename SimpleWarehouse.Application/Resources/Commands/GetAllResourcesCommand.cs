using MediatR;
using SimpleWarehouse.Application.DTOs;

namespace SimpleWarehouse.Application.Resources.Commands;

public record GetAllResourcesCommand : IRequest<Result<IEnumerable<ResourceDto>, ApplicationError>>;