using TaskService.Application.Features.IssueStatuses.Command;
using TaskService.Application.Mediator;
using TaskService.Contracts.IssueStatus;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;


namespace TaskService.Application.Features.IssueStatuses.Handler;

public class IssueStatusCreateHandler(TaskServiceDbContext context) : IRequestHandler<IssueStatusCreateCommand, IssueStatusResponse>
{
    public async Task<IssueStatusResponse> Handle(IssueStatusCreateCommand request, CancellationToken cancellationToken = default)
    {
        Project project = await context.Projects.FindAsync(request.projectId, cancellationToken) ??
            throw new NullReferenceException($"Проект с id: {request.projectId} не найден");

        IssueStatus status = IssueStatus.Create(
            name: request.name,
            type: request.type,
            position: request.position,
            color: request.color,
            projectId: request.projectId);

        await context.IssueStatus.AddAsync(status, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return status.ToResponse();
    }
}
