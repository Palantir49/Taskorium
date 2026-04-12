using System;
using System.Collections.Generic;
using System.Text;

namespace FileStorageService.Application
{
    public class FileUploadResponseDto
    {
        public Guid FileId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public long Size { get; set; }
        public string ContentType { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; }
        public string Message { get; set; } = "File uploaded successfully";
    }
}
