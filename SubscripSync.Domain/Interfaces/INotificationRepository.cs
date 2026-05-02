using SubscripSync.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SubscripSync.Domain.Interfaces
{
    public interface INotificationRepository : IRepository<Notification>
    {
        Task<IEnumerable<Notification>> GetByUserIdAsync(Guid userId);
        Task<int> GetUnreadCountAsync(Guid userId);
    }
}
