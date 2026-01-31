using TaskService.Application.Mediator;
using TaskService.Contracts.Type;

namespace TaskService.Application.Features.Types.Command;

public record class TypeGetByIdQuery(Guid id) : IQuery<TypeResponse>;
