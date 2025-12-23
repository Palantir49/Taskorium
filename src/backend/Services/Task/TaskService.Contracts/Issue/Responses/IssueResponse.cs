namespace TaskService.Contracts.Issue.Responses;

//TODO: а не должен ли тут быть конструктор, который принимает в параметры объект и из него собирает уже свойства?
public record IssueResponse(
    Guid Id,
    string Name,
    Guid ProjectId,
    Guid TaskTypeId,
    Guid TaskStatusId,
    DateTimeOffset CreatedDate,
    string? Description = null,
    Guid? ReporterId = null,
    DateTimeOffset? UpdatedDate = null,
    DateTimeOffset? DueDate = null,
    DateTimeOffset? ResolvedDate = null);
