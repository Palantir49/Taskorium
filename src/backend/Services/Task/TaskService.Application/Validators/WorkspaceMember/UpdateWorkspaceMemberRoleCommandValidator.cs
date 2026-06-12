using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using TaskService.Application.Features.WorkspaceMembers.Write.Command;

namespace TaskService.Application.Validators.WorkspaceMember
{
    public class UpdateWorkspaceMemberRoleCommandValidator : AbstractValidator<UpdateWorkspaceMemberRoleCommand>
    {
    }
}
