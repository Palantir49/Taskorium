using FluentValidation;
using TaskService.Application.Features.IssueStatuses.Command;
using TaskService.Application.Mapping;
using TaskService.Application.Mediator;
using TaskService.Contracts.IssueStatus;
using TaskService.Domain.Entities.Enums;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.IssueStatuses.Handler;

public class IssueStatusUpdateHandler(TaskServiceDbContext context,IValidator<IssueStatusUpdateCommand> validator) 
    : IRequestHandler<IssueStatusUpdateCommand, IssueStatusResponse>
{
    public async Task<IssueStatusResponse> Handle(IssueStatusUpdateCommand request,
        CancellationToken cancellationToken = default)
    {
        await validator.ValidateAsync(request, cancellationToken);  
        var status = await context.IssueStatus.FindAsync([request.Id], cancellationToken) ??
                     throw new NullReferenceException($"Статус с id: {request.Id} не найден");

        status.UpdateName(request.Name);
        status.UpdatePosition(request.Position);
        status.UpdateType(request.Type.ToEntity());

        context.IssueStatus.Update(status);
        await context.SaveChangesAsync(cancellationToken);

        return status.ToResponse();
    }
}
