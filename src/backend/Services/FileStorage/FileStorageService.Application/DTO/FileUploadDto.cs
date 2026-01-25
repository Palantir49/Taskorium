using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace FileStorageService.Application
{
    public class FileUploadDto
    {
        public IFormFile File { get; set; } = null!;
        public string? Description { get; set; }
        public string? Tags { get; set; }
        public string? BucketName { get; set; }
    }
}
