using SubscripSync.Domain.Entities;
using System;
using System.Linq;

namespace SubscripSync.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static void Initialize(SubscripSyncDbContext context)
        {
            context.Database.EnsureCreated();

            // Look for any users.
            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }

            var user = new User
            {
                // Matching the hardcoded ID in Angular for demo
                // userId in DashboardComponent.ts is '3fa85f64-5717-4562-b3fc-2c963f66afa6' but that's a GUID placeholder.
                // Let's use a real GUID and update the Frontend to match, or just use that one.
                // Let's use a specific one.
                Id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"), 
                Email = "demo@subscripsync.com",
                FirstName = "Demo",
                LastName = "User",
                PasswordHash = "HashPlaceholder"
            };

            context.Users.Add(user);

            var subscriptions = new Subscription[]
            {
                new Subscription { UserId = user.Id, Name = "Netflix", Amount = 15.99m, Currency = "USD", BillingCycle = BillingCycle.Monthly, NextPaymentDate = DateTime.UtcNow.AddDays(5), Category = "Entertainment" },
                new Subscription { UserId = user.Id, Name = "Spotify", Amount = 9.99m, Currency = "USD", BillingCycle = BillingCycle.Monthly, NextPaymentDate = DateTime.UtcNow.AddDays(12), Category = "Music" },
                new Subscription { UserId = user.Id, Name = "AWS", Amount = 45.00m, Currency = "USD", BillingCycle = BillingCycle.Monthly, NextPaymentDate = DateTime.UtcNow.AddDays(2), Category = "Infrastructure" },
                new Subscription { UserId = user.Id, Name = "Gym", Amount = 50.00m, Currency = "USD", BillingCycle = BillingCycle.Monthly, NextPaymentDate = DateTime.UtcNow.AddDays(20), Category = "Health" },
                new Subscription { UserId = user.Id, Name = "New York Times", Amount = 4.00m, Currency = "USD", BillingCycle = BillingCycle.Weekly, NextPaymentDate = DateTime.UtcNow.AddDays(3), Category = "News" }
            };

            foreach (var s in subscriptions)
            {
                context.Subscriptions.Add(s);
            }

            context.SaveChanges();
        }
    }
}
