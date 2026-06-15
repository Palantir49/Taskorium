using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Application.Mediator;
using TaskService.Contracts.Issue.Responses;

namespace TaskService.Application.Features.Issues.Command;

public record IssueUpdateStatusCommand(Guid Id, Guid NewStatusId) : IRequest<IssueResponse>;
