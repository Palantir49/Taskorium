namespace TaskService.Application.Cache.Contracts.Project;

public record class ProjectShortMetaCache(
        Guid Id,
        string Name,
        string? Description,
        string Abbreviation);
