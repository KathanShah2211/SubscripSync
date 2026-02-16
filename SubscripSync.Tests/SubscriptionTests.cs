using SubscripSync.Domain.Entities;
using System;
using Xunit;

namespace SubscripSync.Tests
{
    public class SubscriptionTests
    {
        [Fact]
        public void Renew_WeeklySubscription_Adds7Days()
        {
            // Arrange
            var today = new DateTime(2026, 1, 1);
            var sub = new Subscription
            {
                Name = "Weekly News",
                Amount = 10,
                BillingCycle = BillingCycle.Weekly,
                NextPaymentDate = today
            };

            // Act
            sub.Renew();

            // Assert
            Assert.Equal(today.AddDays(7), sub.NextPaymentDate);
        }

        [Fact]
        public void Renew_MonthlySubscription_AddsMonth()
        {
            // Arrange
            var today = new DateTime(2026, 1, 31);
            var sub = new Subscription
            {
                Name = "Monthly Stream",
                Amount = 10,
                BillingCycle = BillingCycle.Monthly,
                NextPaymentDate = today
            };

            // Act
            sub.Renew();

            // Assert
            // 2026 is not a leap year. Feb 28.
            Assert.Equal(new DateTime(2026, 2, 28), sub.NextPaymentDate);
        }

        [Fact]
        public void Renew_YearlySubscription_AddsYear()
        {
            // Arrange
            var today = new DateTime(2026, 1, 1);
            var sub = new Subscription
            {
                Name = "Yearly Cloud",
                Amount = 100,
                BillingCycle = BillingCycle.Yearly,
                NextPaymentDate = today
            };

            // Act
            sub.Renew();

            // Assert
            Assert.Equal(new DateTime(2027, 1, 1), sub.NextPaymentDate);
        }
    }
}
