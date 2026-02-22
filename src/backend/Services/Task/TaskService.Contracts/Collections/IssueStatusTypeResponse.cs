namespace TaskService.Contracts.Collections
{
    public record class IssueStatusTypeResponse(
        int Number,
        string Name,
        string DisplayName
        );
}
