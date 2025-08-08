using MediatR;
using SimpleWarehouse.Application.DTOs;

namespace SimpleWarehouse.Application.Resources.Commands;

public record AddResourceCommand(string Name) : IRequest<Result<IdDto, ApplicationError>>;