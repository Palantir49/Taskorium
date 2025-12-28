using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Application.Interfaces;
using TaskService.Contracts.Workspace.Request;
using TaskService.Contracts.Workspace.Response;
using TaskService.Domain.Entities;
using TaskService.Domain.Repositories;

namespace TaskService.Application.Services
{
    public class WorkspaceService : IWorkspaceService
    {
        IWorkspaceRepository _workspaceRepository;
        public WorkspaceService(IWorkspaceRepository workspaceRepository)
        {
            _workspaceRepository = workspaceRepository;
        }
        public async Task<WorkspaceResponse> CreateWorkspaceAsync(CreateWorkspaceRequest request, CancellationToken ct = default)
        {
            Workspace issue = Workspace.Create(request.Name);
            await _workspaceRepository.AddAsync(issue, ct);
            await _workspaceRepository.SaveChangesAsync();
            return new WorkspaceResponse(issue.Id, issue.Name, issue.CreatedDate, issue.OwnerId);
        }

        public async Task<List<WorkspaceResponse>> GetByOwnerIdAsync(Guid ownerId, CancellationToken ct = default)
        {
            var result = await _workspaceRepository.GetByOwnerIdAsync(ownerId, ct);
            return result.Select(w => new WorkspaceResponse(w.Id, w.Name, w.CreatedDate, w.OwnerId)).ToList();
        }

        public async Task<WorkspaceResponse?> GetWorkspaceByIdAsync(Guid id, CancellationToken ct = default)
        {
            Workspace workspace = await _workspaceRepository.GetByIdAsync(id, ct);
            return new WorkspaceResponse(workspace.Id, workspace.Name, workspace.CreatedDate, workspace.OwnerId);
        }
    }
}
