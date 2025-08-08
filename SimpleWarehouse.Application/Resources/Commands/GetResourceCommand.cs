using MediatR;
using SimpleWarehouse.Application.DTOs;

namespace SimpleWarehouse.Application.Resources.Commands;

public record GetResourceCommand(Guid Id) : IRequest<Result<ResourceDto, ApplicationError>>;