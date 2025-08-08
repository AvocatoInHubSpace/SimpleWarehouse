using MediatR;
using SimpleWarehouse.Application.DTOs;

namespace SimpleWarehouse.Application.Resources.Commands;

public record UpdateResourceCommand(ResourceDto Resource) : IRequest<Result<IdDto, ApplicationError>>;