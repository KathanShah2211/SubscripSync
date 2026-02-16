using System;

namespace SubscripSync.Domain.Common
{
    public abstract class Entity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
        public DateTime? LastModifiedAt { get; protected set; }
    }
}
