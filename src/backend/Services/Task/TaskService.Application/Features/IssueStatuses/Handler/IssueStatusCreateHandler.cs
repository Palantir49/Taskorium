using TaskService.Application.Commands.Projects;
using TaskService.Application.Features.IssueStatuses.Command;
using TaskService.Application.Mediator;
using TaskService.Contracts.IssueStatus;
using TaskService.Domain.Entities;
using TaskService.Domain.Repositories;

namespace TaskService.Application.Features.IssueStatuses.Handler;

public class IssueStatusCreateHandler (IRepositoryWrapper wrapper) : IRequestHandler<IssueStatusCreateCommand, IssueStatusResponse>
{
    public async Task<IssueStatusResponse> Handle(IssueStatusCreateCommand request, CancellationToken cancellationToken = default)
    {
        Project project = await wrapper.Projects.GetByIdAsync(request.projectId, cancellationToken) ??
            throw new NullReferenceException($"Проект с id: {request.projectId} не найден");

        IssueStatus status = IssueStatus.Create(
            name: request.name,
            type: request.type,
            position: request.position,
            color: request.color,
            projectId: request.projectId);

        await wrapper.IssueStatus.AddAsync(status, cancellationToken);
        await wrapper.SaveChangesAsync(cancellationToken);

        return status.ToResponse();
    }
}
