using TaskService.Contracts.IssueStatus;
using TaskService.Contracts.IssueType;
using TaskService.Domain.Entities;

namespace TaskService.Application.Features.IssueStatuses;

public static class IssueStatusMapping
{
    public static IssueStatusResponse ToResponse(IssueStatus issueType)
    {
        return new IssueStatusResponse(
            id: issueType.Id,
            name: issueType.Name.ToString(),
            projectId: issueType.ProjectId,
            type: issueType.Type.ToString(),
            position: issueType.Position,
            color: issueType.Color);
    }
}
