using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Application.Interfaces;
using TaskService.Contracts.Task.Requests;
using TaskService.Contracts.Task.Responses;
using TaskService.Domain.Entities;
using TaskService.Domain.IRepositories;

namespace TaskService.Application.Services
{
    public class IssueService : IIssueService
    {
        private readonly ITaskRepository _taskRepository;

        public IssueService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<TaskResponse> CreateTaskAsync(CreateTaskRequest request, CancellationToken ct = default)
        {
            //TODO: замечал, что указывается конкретно параметр:свойство. как правильно?
            Issue issue = Issue.Create(request.Name, request.Description, request.ProjectId, request.TaskTypeId, request.TaskStatusId, request.ReporterId, request.DueDate);
            await _taskRepository.AddAsync(issue, ct);
            await _taskRepository.SaveChangesAsync();
            return new TaskResponse(issue.Id, issue.Name, issue.ProjectId, issue.TaskTypeId, issue.TaskStatusId, issue.CreatedDate, issue.Description, issue.ReporterId,
                issue.UpdatedDate, issue.DueDate, issue.ResolvedDate);
        }

        public async Task<TaskResponse?> GetTaskByIdAsync(Guid id, CancellationToken ct = default)
        {
            var issue = await _taskRepository.GetByIdAsync(id, ct);
            //TODO: как правильно в этом случае?
            if (issue != null)
            {
                return RespponseMap(issue);
            }
            return null;
        }

        public async Task<List<TaskResponse>> GetByProjectIdAsync(Guid projectId, CancellationToken ct = default)
        {
            var list = await _taskRepository.GetByProjectIdAsync(projectId, ct);
            return list.Select(RespponseMap).ToList();
        }

        private TaskResponse RespponseMap(Issue issue) => new TaskResponse(issue.Id, issue.Name, issue.ProjectId, issue.TaskTypeId, issue.TaskStatusId, 
            issue.CreatedDate, issue.Description, issue.ReporterId, issue.UpdatedDate, issue.DueDate, issue.ResolvedDate);
    }
}
