using TaskService.Application.Features.Collections.Mapping;
using TaskService.Contracts.Attachment;
using TaskService.Contracts.Issue.Responses;
using TaskService.Domain.Entities;

namespace TaskService.Application.Features.Issues.Mapping;

public static class IssueToResponseMapping
{
    public static IssueResponse ToResponse(this Issue issue)
    {
        return new IssueResponse(
            Id: issue.Id,
            Name: issue.Name.ToString(),
            ProjectId: issue.ProjectId,
            TaskStatusId: issue.IssueStatusId,
            CreatedDate: issue.CreatedDate,
            Description: issue.Description,
            IssueType: issue.IssueType.ToResponse(),
            IssuePriority: issue.IssuePriority.ToResponse(),
            UpdatedDate: issue.UpdatedDate,
            DueDate: issue.DueDate,
            ResolvedDate: issue.ResolvedDate,
            attachment: issue.Attachments.Select(x => new AttachmentResponce(Id: x.Id, Name: x.FileName))
            );
    }

    //FAQ: Будет ли у нас частичное получение данных?
    //например, для вывода на доску нам по сути нужно только id, name, status, type,
    //тот же projectId не нужен, ведь по нему идет запрос, но можно вернуть чтоб просто была привязка если нужна
    //для открытия "подробно" по задаче уже нужно все и в том числе комменты и файлы
    //как это будем делить?
}
