namespace TaskService.Application.Cache.Contracts.Workspace;

public record class WorkspaceMetaCache(Guid Id, string Name, DateTimeOffset CreateDate);
