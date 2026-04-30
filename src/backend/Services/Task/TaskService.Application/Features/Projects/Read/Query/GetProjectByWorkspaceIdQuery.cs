using TaskService.Application.Mediator;
using TaskService.Contracts.Project.Responses;

namespace TaskService.Application.Features.Projects.Read.Query;

public record class GetProjectByWorkspaceIdQuery(Guid id) : IQuery<IEnumerable<ProjectResponse>>;
