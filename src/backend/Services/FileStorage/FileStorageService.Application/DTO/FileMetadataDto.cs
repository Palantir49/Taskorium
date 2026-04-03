using System;
using System.Collections.Generic;
using System.Text;

namespace FileStorageService.Application
{
    public class FileMetadataDto
    {
        public Guid Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string OriginalFileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public long Size { get; set; }
        public string? Description { get; set; }
        public string? Tags { get; set; }
        public DateTime UploadedAt { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
