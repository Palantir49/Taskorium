using Microsoft.EntityFrameworkCore;
using TaskService.Application.Exceptions;
using TaskService.Application.Features.Issues.Command;
using TaskService.Application.Mediator;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.Issues.Handler;

public class AddTagToIssueHandler(TaskServiceDbContext context) : IRequestHandler<AddTagToIssueCommand, bool>
{
    public async Task<bool> Handle(AddTagToIssueCommand request, CancellationToken cancellationToken = default)
    {
        Issue issue = await context.Issues.FirstOrDefaultAsync(x => x.Id == request.IssueId, cancellationToken) ??
            throw new KeyNotFoundException($"задача с id: {request.IssueId} не найдена");

        Tag tag = await context.Tags.FirstOrDefaultAsync(x => x.Id == request.TagId, cancellationToken) ??
            throw new KeyNotFoundException($"Таг с id: {request.TagId} не найдена");

        if (tag.ProjectId != issue.ProjectId)
            throw new ConflictException("Задача и таг относятся к разным проектам");

        issue.Tags.Add(tag);
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
