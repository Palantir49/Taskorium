namespace TaskService.Contracts.Common.DTO;

public record ProjectDto
{
    public long Id { get; init; }
    public required string Key { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }

}
