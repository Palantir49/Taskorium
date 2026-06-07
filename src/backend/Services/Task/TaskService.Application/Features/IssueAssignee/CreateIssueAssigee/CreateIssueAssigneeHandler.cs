using Microsoft.EntityFrameworkCore;
using TaskService.Application.Mapping;
using TaskService.Application.Mediator;
using TaskService.Contracts.IssueAssignees;
using TaskService.Domain.Entities;
using TaskService.Domain.Entities.Enums;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.IssueAssignee.CreateIssueAssigee;

public class CreateIssueAssigneeHandler(TaskServiceDbContext context) : IRequestHandler<CreateIssueAssigneeCommand, IssueAssigneesResponce>
{
    public async Task<IssueAssigneesResponce> Handle(CreateIssueAssigneeCommand request, CancellationToken cancellationToken = default)
    {
        if (request.Role == AssigneesRoles.Creator)
            throw new InvalidOperationException("Нельзя назначить создателя");

        bool isAlreadyAssigned = await context.IssueAssignees.AnyAsync(x => x.IssueId == request.IssueId && x.UserId == request.UserId, cancellationToken);

        if (isAlreadyAssigned)
        {
            throw new InvalidOperationException($"Пользователь {request.UserId} уже является ответственным за задачу {request.IssueId}");
        }

        bool userExists = await context.Users.AnyAsync(x => x.Id == request.UserId, cancellationToken);
            
        if(!userExists)
            throw new KeyNotFoundException($"Пользователь не найден");

        bool issueExists = await context.Issues.AnyAsync(x => x.Id == request.IssueId, cancellationToken);

        if (!issueExists) 
            throw new KeyNotFoundException($"Задача не найдена");

        IssueAssignees assignees = IssueAssignees.Create(
            userId: request.UserId,
            issueId: request.IssueId,
            role: request.Role);

        context.Add(assignees);
        await context.SaveChangesAsync();

        return new IssueAssigneesResponce(
            UserId: request.UserId,
            IssueId: request.IssueId,
            Role: assignees.AssigneesRoles.ToDto());
    }
}
