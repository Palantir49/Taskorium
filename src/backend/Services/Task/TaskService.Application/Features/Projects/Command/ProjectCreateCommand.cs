using TaskService.Application.Mediator;
using TaskService.Contracts.Project.Requests;
using TaskService.Contracts.Project.Responses;

namespace TaskService.Application.Commands.Projects.Command
{
    public record ProjectCreateCommand(
    string Name,
    string Description,
    Guid WorkspaceId) : ICommand<ProjectResponse>;
}
