using TaskService.Application.Mediator;
using TaskService.Contracts.Collections;

namespace TaskService.Application.Features.Collections.Query;

public record class GetAllIsssueTypesQuery() : IQuery<IEnumerable<IssueTypeResponse>>;
