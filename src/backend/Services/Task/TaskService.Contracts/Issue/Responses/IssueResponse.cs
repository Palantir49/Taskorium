using TaskService.Contracts.Common.DTO;

namespace TaskService.Contracts.Issue.Responses;

public record IssueResponse
{
     public required Guid Id { get; init; }
     public required string Key { get; init; } // "PROJ-123"
     public required string Summary { get; init; }
     public string? Description { get; init; }
     public int Priority { get; init; }

        
     public ProjectDto? Project { get; init; }
    // public IssueTypeDto? IssueType { get; init; }
   //  public StatusDto? Status { get; init; }
   //  public UserReferenceDto? Reporter { get; init; }
        
  //   public List<IssueAssigneeResponse> Assignees { get; init; } = [];
     
     public DateTime CreatedAt { get; init; }
     public DateTime UpdatedAt { get; init; }
     public DateTime? DueDate { get; init; }
     public DateTime? ResolvedAt { get; init; }
        
     public List<string> Comments { get; init; } = [];
     public int AttachmentCount { get; init; }
     
}
