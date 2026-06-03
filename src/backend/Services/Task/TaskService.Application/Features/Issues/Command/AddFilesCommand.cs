using Microsoft.AspNetCore.Http;
using TaskService.Application.Features.Attachments.Dto;
using TaskService.Application.Mediator;
using TaskService.Contracts.Attachment;

namespace TaskService.Application.Features.Issues.Command;

public record AddFilesCommand(Guid IssueId, List<AttachmentDto> Attachments) : ICommand<IEnumerable<AttachmentResponce>>;
