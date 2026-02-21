using TaskService.Application.Features.IssueTags.Command;
using TaskService.Contracts.IssueTag;
using TaskService.Contracts.IssueTag.Request;
using TaskService.Domain.Entities;

namespace TaskService.Application.Features.IssueTags;

public static class IssueTagMapping
{
    public static IssueTagResponse ToResponse(this IssueTag issueType)
    {
        return new IssueTagResponse(
            id: issueType.Id,
            name: issueType.Name.ToString(),
            projectId: issueType.ProjectId,
            color: issueType.Color);
    }

    public static IssueTagCreateCommand ToCommand(this IssueTagCreateRequest request)
    {
        return new IssueTagCreateCommand(
            name: request.name,
            projectId: request.projectId,
            color: request.color);
    }

    public static IssueTagUpdateCommand IssueTagUpdateCommand(Guid id, IssueTagUpdateRequest request)
    {
        return new IssueTagUpdateCommand(
            id: id,
            name: request.name,
            color: request.color);
    }
}
