using MediatR;
using SimpleWarehouse.Application.DTOs;

namespace SimpleWarehouse.Application.Resources.Commands;

public record DeleteResourceCommand(Guid Id) : IRequest<Result<IdDto, ApplicationError>>;