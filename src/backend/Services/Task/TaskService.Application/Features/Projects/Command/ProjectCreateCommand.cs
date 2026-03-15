using TaskService.Application.Mediator;
using TaskService.Contracts.Project.Responses;

namespace TaskService.Application.Commands.Projects.Command
{
    public record ProjectCreateCommand(
    string Name,
    string Description,
    string Abbreviation,
    Guid WorkspaceId) : ICommand<ProjectResponse>;
}
