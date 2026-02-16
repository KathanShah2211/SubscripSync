using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SubscripSync.Domain.Entities;

namespace SubscripSync.Domain.Interfaces
{
    public interface ISubscriptionRepository : IRepository<Subscription>
    {
        Task<IReadOnlyList<Subscription>> GetByUserIdAsync(Guid userId);
        Task<IReadOnlyList<Subscription>> GetUpcomingRenewalsAsync(DateTime tillDate);
    }
}
