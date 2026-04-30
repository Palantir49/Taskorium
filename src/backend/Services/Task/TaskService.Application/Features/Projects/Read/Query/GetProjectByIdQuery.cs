using TaskService.Application.Mediator;
using TaskService.Contracts.Project.Responses;

namespace TaskService.Application.Features.Projects.Read.Query;

public record class GetProjectByIdQuery(Guid? Id) : IQuery<ProjectResponse>;
