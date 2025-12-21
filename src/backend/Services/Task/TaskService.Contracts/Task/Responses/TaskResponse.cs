using System;
using System.Collections.Generic;
using System.Text;

namespace TaskService.Contracts.Task.Responses
{
    //TODO: а не должен ли тут быть конструктор, который принимает в параметры объект и из него собирает уже свойства?
    public record class TaskResponse(
        Guid id, string name, Guid projectId, Guid taskTypeId, Guid TaskStatusId, DateTimeOffset createdDate,
        string? description = null, Guid? reporterId = null,  DateTimeOffset? updatedDate = null, 
        DateTimeOffset? dueDate = null, DateTimeOffset? resolvedDate = null);
}
