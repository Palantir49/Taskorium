namespace TaskService.Application.Cache;

public static class CacheKeys
{
    //meta - только основные свойства самой сущности без связей
    //User
    public static string User(Guid id) => $"user:{id}:meta";
    public static string UserByKeycloakId(Guid id) => $"keykloak:{id}";

    //Workspace
    public static string Workspace(Guid id) => $"ws:{id}:meta";
    public static string WorkspaceProjects(Guid id) => $"ws:{id}:projects";
    public static string WorkspaceUsers(Guid id) => $"ws:{id}:users";

    //Project
    public static string Project(Guid id) => $"proj:{id}:meta";
    public static string ProjectIssues(Guid id) => $"proj:{id}:issues";
    public static string ProjectTags(Guid id) => $"proj:{id}:tags";
    public static string ProjectStatuses(Guid id) => $"proj:{id}:statuses";
    public static string ProjectUsers(Guid id) => $"proj:{id}:users";

    //кэш issue и ее связанных объектов оставляет кучу вопросов в необходимости.
    //сам issue будет самой активной сущностью в системе. в теории будет 2 чтения - 1 запись
    //tags - является частью project и имеет many-to-many с issue - сложность инвалидации, может слишком сильно замедлить работу
    //comment - в реалии не часто используемая сущность, дак еще и не реализована
    //attachment - имеет смысл на кэш, т.к. скорее всего редко будут прикладывать файлы,
    //но в силу отсутствия кэша остальных сущностей, не имеет смысл и проще вместе со всеми идти через include
    //Assignees - 
}
