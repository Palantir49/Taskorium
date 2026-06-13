using TaskService.Application.Mediator;
using TaskService.Contracts.Project.Responses;

namespace TaskService.Application.Features.Projects.Read.GetProjectByWorkspaceId;

public record class GetProjectByWorkspaceIdQuery(Guid Id) : IQuery<IEnumerable<ProjectResponse>>;
