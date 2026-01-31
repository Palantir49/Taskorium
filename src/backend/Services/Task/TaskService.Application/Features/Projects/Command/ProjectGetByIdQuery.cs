using TaskService.Application.Mediator;
using TaskService.Contracts.Project.Responses;

namespace TaskService.Application.Features.Projects.Command;

public record class ProjectGetByIdQuery(Guid id) : IQuery<ProjectResponse>;
