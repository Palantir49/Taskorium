namespace TaskService.Application.Cache.Contracts.Project
{
    public record class ProjectMetaCache(
        Guid Id,
        string Name,
        string? Description,
        string Abbreviation,
        DateTimeOffset? StartDate,
        DateTimeOffset? FinishDate);
}
