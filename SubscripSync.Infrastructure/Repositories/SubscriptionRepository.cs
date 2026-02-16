using Microsoft.EntityFrameworkCore;
using SubscripSync.Domain.Entities;
using SubscripSync.Domain.Interfaces;
using SubscripSync.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubscripSync.Infrastructure.Repositories
{
    public class SubscriptionRepository : Repository<Subscription>, ISubscriptionRepository
    {
        public SubscriptionRepository(SubscripSyncDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IReadOnlyList<Subscription>> GetByUserIdAsync(Guid userId)
        {
            return await _dbContext.Subscriptions
                .Where(s => s.UserId == userId)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Subscription>> GetUpcomingRenewalsAsync(DateTime tillDate)
        {
            return await _dbContext.Subscriptions
                .Where(s => s.IsActive && s.NextPaymentDate <= tillDate)
                .ToListAsync();
        }
    }
}
