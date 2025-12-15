namespace TaskService.Contracts.Issue.Requests;

public record UpdateIssueRequest
{
    public required string Summary { get; init; }
    
    public string? Description { get; init; }
    
    public long IssueTypeId { get; init; }
    
    public int Priority { get; init; }
        
    public long? StatusId { get; init; }
    
    public List<long>? Assignees { get; init; }
    
    public DateTime? DueDate { get; init; }
        
    public DateTime? ResolvedAt { get; init; }
    
    public string? Resolution { get; init; }
    
    public string? Comments { get; init; }
    
}
