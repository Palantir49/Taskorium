using TaskService.Application.Mediator;

namespace TaskService.Application.Features.Projects.Command;

public record class ProjectDeleteByIdCommand(Guid id) : ICommand<int>;
