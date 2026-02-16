using System;
using SubscripSync.Domain.Common;

namespace SubscripSync.Domain.Entities
{
    public class PaymentHistory : Entity
    {
        public Guid SubscriptionId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Currency { get; set; } = "USD";
        public bool IsSuccess { get; set; } = true;
    }
}
