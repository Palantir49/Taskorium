namespace TaskService.Contracts.Issue.Requests;

public record CreateIssueRequest
{
    public required string Summary { get; init; }
    
    public string? Description { get; init; }
    
    public long IssueTypeId { get; init; }
     
    // more than one assignee
    public List<long>? Assignees { get; init; } = [];

    public int Priority { get; init; } = 3; // Medium by default
    
    public DateTime? DueDate { get; init; }
    
    
}
