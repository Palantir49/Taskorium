namespace TaskService.Contracts.Collections
{
    public record class IssueTypeResponse(
        int Number,
        string Name,
        string DisplayName,
        string Code
        );
}
