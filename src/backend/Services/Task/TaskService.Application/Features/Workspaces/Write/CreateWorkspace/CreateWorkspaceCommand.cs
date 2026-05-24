using TaskService.Application.Mediator;

namespace TaskService.Application.Features.Workspaces.Write.CreateWorkspace;

public record CreateWorkspaceCommand(string Name) : ICommand<CreateWorkspaceResult>;
