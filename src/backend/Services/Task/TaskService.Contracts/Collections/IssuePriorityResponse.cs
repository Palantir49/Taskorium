namespace TaskService.Contracts.Collections;

public record IssuePriorityResponse(
    int Number,
    string Name,
    string DisplayName
    );
