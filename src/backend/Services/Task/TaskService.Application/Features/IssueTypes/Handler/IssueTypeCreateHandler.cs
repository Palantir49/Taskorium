using TaskService.Application.Features.IssueTypes.Command;
using TaskService.Application.Mediator;
using TaskService.Contracts.IssueType;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.IssueTypes.Handler;

public class IssueTypeCreateHandler(TaskServiceDbContext context) : IRequestHandler<IssueTypeCreateCommand, IssueTypeResponse>
{
    public async Task<IssueTypeResponse> Handle(IssueTypeCreateCommand request, CancellationToken cancellationToken = default)
    {
        Project project = await context.Projects.FindAsync(request.projectId, cancellationToken) ??
            throw new NullReferenceException($"Проект с id: {request.projectId} не найден");

        IssueType type = IssueType.Create(
            name: request.name,
            projectId: request.projectId,
            color: request.color);

        await context.IssueType.AddAsync(type, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return type.ToResponse();
    }
}
