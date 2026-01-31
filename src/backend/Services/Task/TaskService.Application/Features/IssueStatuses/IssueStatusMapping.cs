using TaskService.Application.Features.IssueStatuses.Command;
using TaskService.Contracts.IssueStatus;
using TaskService.Contracts.IssueStatus.Request;
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

    public static IssueStatusCreateCommand ToCommand(this IssueStatusCreateRequest request)
    {
        return new IssueStatusCreateCommand(
            name: request.name,
            projectId: request.projectId,
            type: request.type,
            position: request.position,
            color: request.color);
    }

    public static IssueStatusUpdateCommand IssueStatusUpdateCommand(Guid id, IssueStatusUpdateRequest request)
    {
        return new IssueStatusUpdateCommand(
            id: id,
            name: request.name,
            type: request.type,
            position: request.position,
            color: request.color);
    }
}
