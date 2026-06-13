using TaskService.Application.Features.IssueStatuses.Command;
using TaskService.Contracts.Enum;
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
            Name: request.Name,
            ProjectId: request.ProjectId,
            Type: (IssueStatusTypeDto)request.NumberType,
            Position: request.Position,
            Color: request.Color);
    }

    public static IssueStatusUpdateCommand IssueStatusUpdateCommand(Guid id, IssueStatusUpdateRequest request)
    {
        return new IssueStatusUpdateCommand(
            Id: id,
            Name: request.Name,
            Type: (IssueStatusTypeDto)request.NumberType,
            Position: request.Position,
            Color: request.Color);
    }
}
