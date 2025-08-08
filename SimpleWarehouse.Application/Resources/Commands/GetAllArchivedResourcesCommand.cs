using MediatR;
using SimpleWarehouse.Application.DTOs;

namespace SimpleWarehouse.Application.Resources.Commands;

public record GetAllArchivedResourcesCommand : IRequest<Result<IEnumerable<ResourceDto>, ApplicationError>>;