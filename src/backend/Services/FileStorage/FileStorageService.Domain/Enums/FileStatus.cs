namespace FileStorageService.Domain
{
    /// <summary>
    /// Статус файла
    /// </summary>
    public enum FileStatus
    {
        /// <summary>
        /// Загружается
        /// </summary>
        Uploading = 0,

        /// <summary>
        /// Доступен
        /// </summary>
        Available = 1,

        /// <summary>
        /// В обработке
        /// </summary>
        Processing = 2,

        /// <summary>
        /// УДален
        /// </summary>
        Deleted = 3,

        /// <summary>
        /// Ошибка
        /// </summary>
        Error = 4
    }
}
