using TaskService.Application.Mediator;
using TaskService.Contracts.Project.Responses;

namespace TaskService.Application.Features.Projects.Write.CreateProject
{
    public record CreateProjectCommand(
    string Name,
    string Description,
    string Abbreviation,
    Guid WorkspaceId,
    Guid UserId) : ICommand<ProjectResponse>;
}
