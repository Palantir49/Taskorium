using Microsoft.EntityFrameworkCore;
using TaskService.Application.Features.IssueStatuses.Command;
using TaskService.Application.Mediator;
using TaskService.Contracts.IssueStatus;
using TaskService.Domain.Entities;
using TaskService.Domain.Entities.Enums;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.IssueStatuses.Handler;

public class IssueStatusCreateHandler(TaskServiceDbContext context) : IRequestHandler<IssueStatusCreateCommand, IssueStatusResponse>
{
    public async Task<IssueStatusResponse> Handle(IssueStatusCreateCommand request, CancellationToken cancellationToken = default)
    {
        if((IssueStatusType)request.numberType == IssueStatusType.Initial)
        {
            throw new Exception("В проекте не может существовать больше одного статуса инициализации задачи");
        }

        Project project = await context.Projects
            //.Include(p => p.Statuses)
            .FirstOrDefaultAsync(x => x.Id == request.projectId, cancellationToken) ?? 
            throw new NullReferenceException($"Проект с id: {request.projectId} не найден");        

        IssueStatus status = IssueStatus.Create(
            name: request.name,
            numberType: request.numberType,
            position: request.position,
            color: request.color,
            projectId: request.projectId);

        await context.IssueStatus.AddAsync(status, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return status.ToResponse();
    }
}
