namespace TaskService.Application.Features.Attachments.Dto;

public record AttachmentDto
{
    public required string Name { get; set; }

    public required Stream Content { get; set; }

    public long ContentLength { get; set; }

    public required string ContentType { get; set; }
}
