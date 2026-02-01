using System;
using System.Collections.Generic;
using System.Text;

namespace TaskService.Application.Features.Users.Delete;

public record DeleteUserByIdResult(Guid id,Guid keycloakId, string email, string userName);
