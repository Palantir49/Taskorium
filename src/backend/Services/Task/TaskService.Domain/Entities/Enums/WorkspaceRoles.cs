using System;
using System.Collections.Generic;
using System.Text;

namespace TaskService.Domain.Entities.Enums
{
    public enum WorkspaceRoles
    {
        Creator = 0, // полный доступ в созданной сущности и в ее потомках
        Admin = 1, //имеет доступ ко всем CRUD, может назначать наблюдателей и исполнителей в созданной сущности и ее потомках
        Viewer = 2 //имеет доступ только на чтение данных.
    }
}
