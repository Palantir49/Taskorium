using TaskService.Application.Mediator;
using TaskService.Contracts.Project.Responses;

namespace TaskService.Application.Features.Projects.Command;

public record ProjectUpdateCommand(
    Guid id,
    string Name,
    string Description,
    Guid WorkspaceId) : ICommand<ProjectResponse>;
