using Microsoft.AspNetCore.Http;

namespace TaskService.Contracts.Issue.Requests;

public record AddFilesRequest(List<IFormFile> Attachments);
