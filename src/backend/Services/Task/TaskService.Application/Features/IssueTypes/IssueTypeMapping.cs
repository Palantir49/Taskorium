using TaskService.Contracts.IssueType;
using TaskService.Domain.Entities;

namespace TaskService.Application.Features.IssueTypes
{
    public static class IssueTypeMapping
    {
        public static IssueTypeResponse ToResponse(IssueType issueType)
        {
            return new IssueTypeResponse(
                id: issueType.Id,
                name: issueType.Name.ToString(),
                projectId: issueType.ProjectId,
                color: issueType.Color);
        }
    }
}
