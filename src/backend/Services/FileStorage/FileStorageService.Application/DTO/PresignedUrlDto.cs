using System;
using System.Collections.Generic;
using System.Text;

namespace FileStorageService.Application
{
    public class PresignedUrlDto
    {
        public string Url { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
}
