using TaskService.Application.Mediator;

namespace TaskService.Application.Features.Projects.Write.DeleteProjectById;

public record class DeleteProjectByIdCommand(Guid id) : ICommand<int>;
