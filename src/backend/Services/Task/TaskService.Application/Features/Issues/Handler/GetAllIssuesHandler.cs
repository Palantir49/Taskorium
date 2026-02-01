using System.Collections.Generic;
using TaskService.Application.Commands.Issues.Query;
using TaskService.Application.Mediator;
using TaskService.Contracts.Issue.Responses;
using TaskService.Domain.Repositories;

namespace TaskService.Application.Commands.Issues.Handler;

public class GetAllIssuesHandler : IRequestHandler<GetAllIssuesQuery, IssuesResponse>
{
    private readonly IRepositoryWrapper _wrapper;

    public GetAllIssuesHandler(IRepositoryWrapper wrapper)
    {
        _wrapper = wrapper;
    }

    public async Task<IssuesResponse> Handle(GetAllIssuesQuery query, CancellationToken cancellationToken = default)
    {
        // Mock data for testing without database
        var mockIssues = new List<IssueResponse>
        {
            new IssueResponse(
                Id: Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Name: "Implement user authentication",
                ProjectId: Guid.Parse("22222222-2222-2222-2222-222222222222"),
                TaskTypeId: Guid.Parse("33333333-3333-3333-3333-333333333333"),
                TaskStatusId: Guid.Parse("44444444-4444-4444-4444-444444444444"),
                CreatedDate: DateTimeOffset.UtcNow.AddDays(-5),
                Description: "Add JWT authentication for users",
                //ReporterId: Guid.Parse("55555555-5555-5555-5555-555555555555"),
                UpdatedDate: DateTimeOffset.UtcNow.AddDays(-2),
                DueDate: DateTimeOffset.UtcNow.AddDays(10),
                ResolvedDate: null
            ),
            new IssueResponse(
                Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                Name: "Create Kanban board UI",
                ProjectId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                TaskTypeId: Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                TaskStatusId: Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                CreatedDate: DateTimeOffset.UtcNow.AddDays(-3),
                Description: "Design and implement Kanban board component",
                //ReporterId: Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                UpdatedDate: DateTimeOffset.UtcNow.AddDays(-1),
                DueDate: DateTimeOffset.UtcNow.AddDays(7),
                ResolvedDate: null
            ),
            new IssueResponse(
                Id: Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                Name: "Fix database connection bug",
                ProjectId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                TaskTypeId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                TaskStatusId: Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                CreatedDate: DateTimeOffset.UtcNow.AddDays(-7),
                Description: "Resolve PostgreSQL connection timeout issues",
                //ReporterId: Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                UpdatedDate: DateTimeOffset.UtcNow,
                DueDate: DateTimeOffset.UtcNow.AddDays(2),
                ResolvedDate: DateTimeOffset.UtcNow
            )
        };

        return new IssuesResponse(mockIssues);

        // Original database code (commented out)
        /*
        var issues = await _wrapper.Issues.GetListByConditionAsync(null, null, cancellationToken);
        if (issues == null)
        {
            return new IssuesResponse(new List<IssueResponse>());
        }

        var responses = issues.Select(issue => new IssueResponse(
            Id: issue.Id,
            Name: issue.Name.ToString(),
            ProjectId: issue.ProjectId,
            TaskTypeId: issue.IssueTypeId,
            TaskStatusId: issue.IssueStatusId,
            CreatedDate: issue.CreatedDate,
            Description: issue.Description,
            UpdatedDate: issue.UpdatedDate,
            DueDate: issue.DueDate,
            ResolvedDate: issue.ResolvedDate
        )).ToList();

        return new IssuesResponse(responses);
        */
    }
}
