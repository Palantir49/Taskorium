using TaskService.Application.Features.IssueStatuses.Command;
using TaskService.Contracts.IssueStatus;
using TaskService.Contracts.IssueStatus.Request;
using TaskService.Domain.Entities;

namespace TaskService.Application.Features.IssueStatuses;

public static class IssueStatusMapping
{
    public static IssueStatusResponse ToResponse(this IssueStatus issueStatus)
    {
        return new IssueStatusResponse(
            id: issueStatus.Id,
            name: issueStatus.Name.ToString(),
            projectId: issueStatus.ProjectId,
            type: issueStatus.Type.ToString(),
            position: issueStatus.Position);
    }

    public static IssueStatusCreateCommand ToCommand(this IssueStatusCreateRequest request)
    {
        return new IssueStatusCreateCommand(
            name: request.name,
            projectId: request.projectId,
            numberType: request.numberType,
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
