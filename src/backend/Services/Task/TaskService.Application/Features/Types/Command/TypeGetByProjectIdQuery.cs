using TaskService.Application.Mediator;
using TaskService.Contracts.Type;

namespace TaskService.Application.Features.Types.Command;

public record class TypeGetByProjectIdQuery(Guid id) : IQuery<TypeResponse>;
