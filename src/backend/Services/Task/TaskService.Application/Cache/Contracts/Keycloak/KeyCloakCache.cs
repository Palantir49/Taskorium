namespace TaskService.Application.Cache.Contracts.Keycloak;

public record KeyCloakCache(Guid id,
    List<WorkspaceMemberCache> WorkspaceMembers,
    List<ProjectMemberCache> ProjectMembers);
