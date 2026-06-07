using TaskService.Application.Features.Collections.Mapping;
using TaskService.Application.Mapping;
using TaskService.Contracts.Attachment;
using TaskService.Contracts.Common.DTO;
using TaskService.Contracts.Issue.Responses;
using TaskService.Domain.Entities;

namespace TaskService.Application.Features.Issues.Mapping;

public static class IssueToResponseMapping
{
    public static IssueResponse ToResponse(this Issue issue)
    {
        return new IssueResponse(
            issue.Id,
            issue.Name.ToString(),
            ProjectId: issue.ProjectId,
            TaskStatusId: issue.IssueStatusId,
            CreatedDate: issue.CreatedDate,
            Description: issue.Description,
            IssueType: issue.IssueType.ToResponse(),
            IssuePriority: issue.IssuePriority.ToResponse(),
            UpdatedDate: issue.UpdatedDate,
            DueDate: issue.DueDate,
            ResolvedDate: issue.ResolvedDate,
            Attachments: issue.Attachments.Select(x => new AttachmentResponce(x.Id, x.FileName)),
            Assignees: issue.IssueAssignees.Select(x =>
                new IssueAssigneesDto(x.UserId, x.Role.ToDto(), x.User.Username.ToString()))
        );
    }

    //FAQ: Будет ли у нас частичное получение данных?
    //например, для вывода на доску нам по сути нужно только id, name, status, type,
    //тот же projectId не нужен, ведь по нему идет запрос, но можно вернуть чтоб просто была привязка если нужна
    //для открытия "подробно" по задаче уже нужно все и в том числе комменты и файлы
    //как это будем делить?
}
