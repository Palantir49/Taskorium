using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TaskService.Application.Features.IssueStatuses.Command;
using TaskService.Application.Mapping;
using TaskService.Application.Mediator;
using TaskService.Contracts.IssueStatus;
using TaskService.Domain.Entities;
using TaskService.Domain.Entities.Enums;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.IssueStatuses.Handler;

public class IssueStatusCreateHandler(TaskServiceDbContext context, IValidator<IssueStatusCreateCommand> validator) : IRequestHandler<IssueStatusCreateCommand, IssueStatusResponse>
{
    public async Task<IssueStatusResponse> Handle(IssueStatusCreateCommand request, CancellationToken cancellationToken = default)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);
        if ((IssueStatusType)request.Type == IssueStatusType.Initial)
        {
            throw new Exception("В проекте не может существовать больше одного статуса инициализации задачи");
        }

        Project project = await context.Projects
            //.Include(p => p.Statuses)
            .FirstOrDefaultAsync(x => x.Id == request.ProjectId, cancellationToken) ??
            throw new NullReferenceException($"Проект с id: {request.ProjectId} не найден");

        IssueStatus status = IssueStatus.Create(
            name: request.Name,
            type: request.Type.ToEntity(),
            position: request.Position,
            projectId: request.ProjectId,
            color: request.Color);

        await context.IssueStatus.AddAsync(status, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return status.ToResponse();
    }
}
