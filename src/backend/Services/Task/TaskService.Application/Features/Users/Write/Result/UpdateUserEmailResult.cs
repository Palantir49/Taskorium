using System;
using System.Collections.Generic;
using System.Text;

namespace TaskService.Application.Features.Users.Write.Result;

public record UpdateUserEmailResult(Guid id, Guid keycloakId, string email, string userName);
