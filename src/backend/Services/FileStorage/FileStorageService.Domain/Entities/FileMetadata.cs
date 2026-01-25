using System;
using System.Collections.Generic;
using System.Text;

namespace FileStorageService.Domain
{
    public class FileMetadata
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Наименование
        /// </summary>
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// Оригинальное наименование
        /// </summary>
        public string OriginalFileName { get; set; } = string.Empty;

        /// <summary>
        /// Тип содержимого
        /// </summary>
        public string ContentType { get; set; } = string.Empty;

        /// <summary>
        /// Размер
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// Бакет
        /// </summary>
        public string BucketName { get; set; } = string.Empty;

        /// <summary>
        /// Описание
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Объект
        /// </summary>
        public string ObjectName { get; set; } = string.Empty;

        /// <summary>
        /// Тэги
        /// </summary>
        public string? Tags { get; set; }

        /// <summary>
        /// Пользователь
        /// </summary>
        public Guid UploadedBy { get; set; }

        /// <summary>
        /// Создан
        /// </summary>
        public DateTime UploadedAt { get; set; }

        /// <summary>
        /// Изменен
        /// </summary>
        public DateTime? ModifiedAt { get; set; }

        /// <summary>
        /// Удален
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Хэш
        /// </summary>
        public string? Hash { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public FileStatus Status { get; set; }
    }
}
