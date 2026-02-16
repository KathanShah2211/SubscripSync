using MediatR;
using SubscripSync.Domain.Entities;
using SubscripSync.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SubscripSync.Application.Subscriptions.Commands.CreateSubscription
{
    public class CreateSubscriptionCommand : IRequest<Guid>
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "USD";
        public BillingCycle BillingCycle { get; set; }
        public DateTime NextPaymentDate { get; set; }
        public string Category { get; set; }
    }

    public class CreateSubscriptionCommandHandler : IRequestHandler<CreateSubscriptionCommand, Guid>
    {
        private readonly IRepository<Subscription> _repository;

        public CreateSubscriptionCommandHandler(IRepository<Subscription> repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(CreateSubscriptionCommand request, CancellationToken cancellationToken)
        {
            var subscription = new Subscription
            {
                UserId = request.UserId,
                Name = request.Name,
                Amount = request.Amount,
                Currency = request.Currency,
                BillingCycle = request.BillingCycle,
                NextPaymentDate = request.NextPaymentDate,
                Category = request.Category,
                IsActive = true
            };

            await _repository.AddAsync(subscription);
            return subscription.Id;
        }
    }
}
