using TaskService.Application.Features.IssueTypes.Command;
using TaskService.Contracts.IssueType;
using TaskService.Contracts.IssueType.Request;
using TaskService.Domain.Entities;

namespace TaskService.Application.Features.IssueTypes;

public static class IssueTypeMapping
{
    public static IssueTypeResponse ToResponse(this IssueType issueType)
    {
        return new IssueTypeResponse(
            id: issueType.Id,
            name: issueType.Name.ToString(),
            projectId: issueType.ProjectId,
            color: issueType.Color);
    }

    public static IssueTypeCreateCommand ToCommand(IssueTypeCreateRequest request)
    {
        return new IssueTypeCreateCommand(
            name: request.name,
            projectId: request.projectId,
            color: request.color);
    }

    public static IssueTypeUpdateCommand IssueTypeUpdateCommand(Guid id, IssueTypeUpdateRequest request)
    {
        return new IssueTypeUpdateCommand(
            id: id,
            name: request.name,
            color: request.color);
    }
}
