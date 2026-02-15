using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Domain.Entities;

namespace TaskService.Application.Features.Users.Get;

public record GetAllUsersResult(IEnumerable<User> users);
