using FileStorageService.Application;
using Microsoft.AspNetCore.Mvc;

namespace FileStorageService.API;

[ApiController]
[Route("api/[controller]")]
public class FilesController : ControllerBase
{
    private readonly IFileStorageService _fileStorageService;
    private readonly ILogger<FilesController> _logger;

    public FilesController(IFileStorageService fileStorageService, ILogger<FilesController> logger)
    {
        _fileStorageService = fileStorageService;
        _logger = logger;
    }

    /// <summary>
    /// Upload a file
    /// </summary>
    [HttpPost("upload")]
    [ProducesResponseType(typeof(FileUploadResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<FileUploadResponseDto>> UploadFile(
        [FromForm] FileUploadDto fileUpload,
        CancellationToken cancellationToken)
    {
        // TODO: Get user ID from authentication context
        var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

        var result = await _fileStorageService.UploadFileAsync(fileUpload, userId, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Download a file
    /// </summary>
    [HttpGet("{fileId}/download")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DownloadFile(Guid fileId, CancellationToken cancellationToken)
    {
        try
        {
            var file = await _fileStorageService.DownloadFileAsync(fileId, cancellationToken);
            return File(file.FileStream, file.ContentType, file.FileName);
        }
        catch (FileNotFoundException)
        {
            return NotFound(new { message = "File not found" });
        }
    }

    /// <summary>
    /// Get file metadata
    /// </summary>
    [HttpGet("{fileId}")]
    [ProducesResponseType(typeof(FileMetadataDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FileMetadataDto>> GetFileMetadata(Guid fileId, CancellationToken cancellationToken)
    {
        var metadata = await _fileStorageService.GetFileMetadataAsync(fileId, cancellationToken);

        if (metadata == null)
        {
            return NotFound(new { message = "File not found" });
        }

        return Ok(metadata);
    }

    /// <summary>
    /// Get all files for current user
    /// </summary>
    [HttpGet("my-files")]
    [ProducesResponseType(typeof(IEnumerable<FileMetadataDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<FileMetadataDto>>> GetMyFiles(CancellationToken cancellationToken)
    {
        // TODO: Get user ID from authentication context
        var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");

        var files = await _fileStorageService.GetUserFilesAsync(userId, cancellationToken);
        return Ok(files);
    }

    /// <summary>
    /// Delete a file
    /// </summary>
    [HttpDelete("{fileId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteFile(Guid fileId, CancellationToken cancellationToken)
    {
        var result = await _fileStorageService.DeleteFileAsync(fileId, cancellationToken);

        if (!result)
        {
            return NotFound(new { message = "File not found" });
        }

        return NoContent();
    }

    /// <summary>
    /// Get presigned URL for file access
    /// </summary>
    [HttpGet("{fileId}/presigned-url")]
    [ProducesResponseType(typeof(PresignedUrlDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PresignedUrlDto>> GetPresignedUrl(
        Guid fileId,
        [FromQuery] int expiryMinutes = 60,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _fileStorageService.GetPresignedUrlAsync(fileId, expiryMinutes, cancellationToken);
            return Ok(result);
        }
        catch (FileNotFoundException)
        {
            return NotFound(new { message = "File not found" });
        }
    }
}
