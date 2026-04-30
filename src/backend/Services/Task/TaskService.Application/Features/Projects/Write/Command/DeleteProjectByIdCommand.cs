using TaskService.Application.Mediator;

namespace TaskService.Application.Features.Projects.Write.Command;

public record class DeleteProjectByIdCommand(Guid id) : ICommand<int>;
