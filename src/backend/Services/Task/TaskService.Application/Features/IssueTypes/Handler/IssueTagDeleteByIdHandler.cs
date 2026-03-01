using System.Net.NetworkInformation;
using Microsoft.EntityFrameworkCore;
using TaskService.Application.Features.IssueTags.Command;
using TaskService.Application.Mediator;
using TaskService.Contracts.IssueTag;
using TaskService.Domain.Entities;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Application.Features.IssueTags.Handler
{
    public class IssueTagDeleteByIdHandler(TaskServiceDbContext context) : IRequestHandler<IssueTagDeleteByIdCommand, int>
    {
        public async Task<int> Handle(IssueTagDeleteByIdCommand request, CancellationToken cancellationToken = default)
        {
            IssueTag tag = await context.IssueTag.FindAsync(request.id, cancellationToken) ??
            throw new NullReferenceException($"Тип задачи с id: {request.id} не найден");

            //List<Issue> issue = await context.Issues
            //    .Where(x => x.IssueTagId == tag.Id).ToListAsync(cancellationToken);

            //if (issue != null && issue.Count > 0)
            //    throw new NullReferenceException($"Нельзя удалить тип задачи, пока существуют связанные задачи");

            //context.IssueTag.Remove(tag);
            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}
