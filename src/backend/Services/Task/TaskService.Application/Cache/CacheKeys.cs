namespace TaskService.Application.Cache;

public static class CacheKeys
{
    //meta - только основные свойства самой сущности без связей
    //tag - когда удаляется вся сущность, для инвалдации всего связанного кэша
    //meta tag - для инвалидации meta и short meta при изменении meta

    //keycloak
    public static string KeycloakTag(Guid id) => $"keycloak:{id}";

    public static string KeycloakMeta(Guid id) => $"keycloak:{id}:meta";
    //public static string KeycloakWorkspace(Guid id) => $"keycloak:{id}:ws";
    //public static string KeycloakProject(Guid id) => $"keycloak:{id}:proj";

    //User
    public static string UserTag(Guid id) => $"user:{id}";
    public static string UserProjTag(Guid id) => $"user:{id}:proj";

    public static string UserMeta(Guid id) => $"user:{id}:meta";
    public static string UserWorkspaces(Guid id) => $"user:{id}:ws";

    public static string WorkspaceProjects(Guid wsId, Guid userId) => $"ws:{wsId}:user:{userId}:projects";

    //Workspace
    public static string WorkspaceTag(Guid id) => $"ws:{id}";
    public static string WorkspaceProjTag(Guid id) => $"ws:{id}:proj";

    public static string WorkspaceMeta(Guid id) => $"ws:{id}:meta";
    public static string WorkspaceUsers(Guid id) => $"ws:{id}:users";

    //Project
    public static string ProjectTag(Guid id) => $"proj:{id}";

    public static string ProjectMeta(Guid id) => $"proj:{id}:meta";
    public static string ProjectIssues(Guid id) => $"proj:{id}:issues";
    public static string ProjectIssueTags(Guid id) => $"proj:{id}:tags";
    public static string ProjectStatuses(Guid id) => $"proj:{id}:statuses";
    public static string ProjectUsers(Guid id) => $"proj:{id}:users";
}
