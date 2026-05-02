using System;
using SubscripSync.Domain.Common;

namespace SubscripSync.Domain.Entities
{
    public class Notification : Entity
    {
        public Guid UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public NotificationType Type { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public enum NotificationType
    {
        Info,
        Warning,
        Alert
    }
}
