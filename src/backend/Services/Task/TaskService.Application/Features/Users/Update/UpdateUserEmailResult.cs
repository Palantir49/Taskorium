using System;
using System.Collections.Generic;
using System.Text;

namespace TaskService.Application.Features.Users.Update;

public record UpdateUserEmailResult(Guid id,Guid keycloakId, string email, string userName);
