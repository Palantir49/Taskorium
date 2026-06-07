using TaskService.Contracts.Enum;

namespace TaskService.Contracts.Common.DTO;

public record IssueAssigneesDto(Guid UserId, AssigneesRolesDto ProjectRolesDto, string? UserName = null);
