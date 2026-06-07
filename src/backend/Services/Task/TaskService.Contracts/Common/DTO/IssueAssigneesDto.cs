using TaskService.Contracts.Enum;

namespace TaskService.Contracts.Common.DTO;

public record IssueAssigneesDto(Guid UserId, ProjectRolesDto ProjectRolesDto, string? UserName = null);
