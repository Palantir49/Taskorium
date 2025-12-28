namespace TaskService.Contracts.Issue.Requests;

//TODO: уточнить - ведь логично тогда разбивать на несколько DTO?
//Смена статуса, типа и т.д. скорее всего будет происходить отдельно
//и не захочется каждый раз отправлять всю форму
//Написать Вадиму
public record class UpdateIssueRequest(
    Guid Id,
    string Name,
    Guid? ProjectId = null,
    Guid? TaskTypeId = null,
    Guid? TaskStatusId = null,
    string? Description = null,
    Guid? ReporterId = null,
    DateTimeOffset? DueDate = null);
