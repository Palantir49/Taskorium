namespace TaskService.Application.Features.Issues.Dto;

public record AttachmentDto
{
    public required string Name { get; set; }

    public required Stream Content { get; set; }

    public long ContentLength { get; set; }

    public required string ContentType { get; set; }
}
