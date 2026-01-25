using System;
using System.Collections.Generic;
using System.Text;

namespace FileStorageService.Application
{
    public class FileDownloadDto
    {
        public Stream FileStream { get; set; } = null!;
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public long Size { get; set; }
    }
}
