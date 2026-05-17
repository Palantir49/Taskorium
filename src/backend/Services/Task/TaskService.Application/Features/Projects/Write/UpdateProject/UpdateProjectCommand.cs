using TaskService.Application.Mediator;
using TaskService.Contracts.Project.Responses;

namespace TaskService.Application.Features.Projects.Write.UpdateProject;

public record UpdateProjectCommand(
    Guid id,
    string Name,
    string Description) : ICommand<ProjectResponse>;
