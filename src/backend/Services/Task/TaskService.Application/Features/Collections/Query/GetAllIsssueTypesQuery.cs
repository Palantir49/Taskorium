using TaskService.Application.Mediator;
using TaskService.Contracts.Collections;
using TaskService.Contracts.Issue.Responses;

namespace TaskService.Application.Features.Collections.Query;

public record class GetAllIsssueTypesQuery() : IQuery<IEnumerable<IssueTypeResponse>>;
