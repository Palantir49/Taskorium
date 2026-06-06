using TaskService.Contracts.Enum;
using TaskService.Domain.Entities.Enums;

namespace TaskService.Application.Mapping;

public static class AssigneeRoleMap
{
    extension(AssigneesRoles domainEnum)
    {
        public AssigneesRolesDto ToDto()
        {
            switch (domainEnum)
            {
                case AssigneesRoles.Admin:
                    return AssigneesRolesDto.Admin;
                case AssigneesRoles.Member:
                    return AssigneesRolesDto.Member;
                case AssigneesRoles.Creator:
                    return AssigneesRolesDto.Creator;
                default:
                    throw new InvalidOperationException("Ошибка маппирования ролей ответственных");
            }
        }
    }
    extension(AssigneesRolesDto domainEnum)
    {
        public AssigneesRoles ToEntity()
        {
            switch (domainEnum)
            {
                case AssigneesRolesDto.Admin:
                    return AssigneesRoles.Admin;
                case AssigneesRolesDto.Member:
                    return AssigneesRoles.Member;
                case AssigneesRolesDto.Creator:
                    return AssigneesRoles.Creator;
                default:
                    throw new InvalidOperationException("Ошибка маппирования ролей ответственных");
            }
        }
    }
}
