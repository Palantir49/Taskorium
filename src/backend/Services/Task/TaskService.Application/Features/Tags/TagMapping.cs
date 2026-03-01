using TaskService.Application.Features.Tags.Command;
using TaskService.Contracts.Tag;
using TaskService.Contracts.Tag.Request;
using TaskService.Domain.Entities;

namespace TaskService.Application.Features.Tags;

public static class TagMapping
{
    public static TagResponse ToResponse(this Tag issueType)
    {
        return new TagResponse(
            id: issueType.Id,
            name: issueType.Name.ToString(),
            projectId: issueType.ProjectId);
    }

    public static TagCreateCommand ToCommand(this TagCreateRequest request)
    {
        return new TagCreateCommand(
            name: request.name,
            projectId: request.projectId);
    }

    public static TagUpdateCommand TagUpdateCommand(Guid id, TagUpdateRequest request)
    {
        return new TagUpdateCommand(
            id: id,
            name: request.name);
    }
}
