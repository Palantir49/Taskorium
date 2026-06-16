using System;
using System.Collections.Generic;
using System.Text;

namespace TaskService.Contracts.Issue.Requests;

public record IssueUpdateStatusRequest(Guid NewStatusId);
