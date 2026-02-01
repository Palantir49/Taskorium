namespace TaskService.Contracts.Issue.Requests;

/// <summary>
/// Запрос на создание задачи
/// </summary>
/// <param name="Name">Наименование задачи.</param>
/// <param name="ProjectId">Идентификатор проекта.</param>
/// <param name="TaskTypeId">Идентификатор типа задачи. Должен быть создан в указаном проекте.</param>
/// <param name="TaskStatusId">Идентификатор статуса задачи. Должен быть создан в указаном проекте.</param>
/// <param name="Description">Описание задачи.</param>
/// <param name="DueDate">Дата ожидаемого завершения задачи.</param>
public record class CreateIssueRequest(
    string Name,
    Guid ProjectId,
    Guid TaskTypeId,
    Guid TaskStatusId,
    string? Description = null,
    DateTimeOffset? DueDate = null);
