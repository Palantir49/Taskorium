using System.Collections.Generic;

namespace TaskService.Contracts.Issue.Responses;

public record IssuesResponse(List<IssueResponse> Issues);