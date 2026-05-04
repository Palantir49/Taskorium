using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Contracts.User.Responses;
using TaskService.Domain.Entities;

namespace TaskService.Application.Features.Users.Read.Result;

public record GetUsersPageResult(List<UserResponse> users);
