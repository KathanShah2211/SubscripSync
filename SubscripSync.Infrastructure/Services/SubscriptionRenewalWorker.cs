using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SubscripSync.Domain.Entities;
using SubscripSync.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SubscripSync.Infrastructure.Services
{
    public class SubscriptionRenewalWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SubscriptionRenewalWorker> _logger;

        public SubscriptionRenewalWorker(IServiceProvider serviceProvider, ILogger<SubscriptionRenewalWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Checking for subscription renewals at: {time}", DateTimeOffset.Now);

                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var subscriptionRepo = scope.ServiceProvider.GetRequiredService<ISubscriptionRepository>();
                        var repository = scope.ServiceProvider.GetRequiredService<IRepository<PaymentHistory>>();

                        var renewals = await subscriptionRepo.GetUpcomingRenewalsAsync(DateTime.UtcNow);

                        foreach (var sub in renewals)
                        {
                            _logger.LogInformation("Renewing subscription: {id} - {name}", sub.Id, sub.Name);

                            // Create Payment Record (Simulated)
                            var payment = new PaymentHistory
                            {
                                SubscriptionId = sub.Id,
                                Amount = sub.Amount,
                                PaymentDate = DateTime.UtcNow,
                                Currency = sub.Currency,
                                IsSuccess = true
                            };
                            await repository.AddAsync(payment);

                            // Update Next Payment Date
                            sub.Renew();
                            await subscriptionRepo.UpdateAsync(sub);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred executing subscription renewal.");
                }

                // Check every minute for demo (in production, maybe daily)
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
