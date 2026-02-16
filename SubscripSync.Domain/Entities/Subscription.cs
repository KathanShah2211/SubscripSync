using System;
using SubscripSync.Domain.Common;

namespace SubscripSync.Domain.Entities
{
    public class Subscription : Entity
    {
        public Guid UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "USD";
        public BillingCycle BillingCycle { get; set; }
        public DateTime NextPaymentDate { get; set; }
        public string Category { get; set; } = "Uncategorized";
        public bool IsActive { get; set; } = true;

        public void Renew()
        {
            switch (BillingCycle)
            {
                case BillingCycle.Weekly:
                    NextPaymentDate = NextPaymentDate.AddDays(7);
                    break;
                case BillingCycle.Monthly:
                    NextPaymentDate = NextPaymentDate.AddMonths(1);
                    break;
                case BillingCycle.Yearly:
                    NextPaymentDate = NextPaymentDate.AddYears(1);
                    break;
            }
        }
    }

    public enum BillingCycle
    {
        Weekly,
        Monthly,
        Yearly
    }
}
