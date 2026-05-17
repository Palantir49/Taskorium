using System;
using System.Collections.Generic;
using System.Text;

namespace TaskService.Application.Features.Users.Write.DeleteUserById;

public record DeleteUserByIdResult(Guid id, Guid keycloakId, string email, string userName);
