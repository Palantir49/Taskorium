using System;
using System.Collections.Generic;
using System.Text;

namespace TaskService.Infrastructure.Outbox.Models
{
    /// <summary>
    /// Статусы outbox-сообщения.
    /// </summary>
    public static class OutboxStatuses
    {
        public const string Pending = "Pending";
        public const string Processed = "Processed";
        public const string Failed = "Failed";
    }
}
