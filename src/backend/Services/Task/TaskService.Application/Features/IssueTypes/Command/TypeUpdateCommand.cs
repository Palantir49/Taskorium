using TaskService.Application.Mediator;

namespace TaskService.Application.Features.Types.Command;

public record class TypeUpdateCommand(
    string name,
    Guid projectId,
    string color) : ICommand<int>;
