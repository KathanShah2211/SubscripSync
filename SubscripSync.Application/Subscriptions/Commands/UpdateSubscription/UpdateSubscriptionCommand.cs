using MediatR;
using SubscripSync.Domain.Entities;
using SubscripSync.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SubscripSync.Application.Subscriptions.Commands.UpdateSubscription
{
    public class UpdateSubscriptionCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public BillingCycle BillingCycle { get; set; }
        public DateTime NextPaymentDate { get; set; }
        public string Category { get; set; }
    }

    public class UpdateSubscriptionCommandHandler : IRequestHandler<UpdateSubscriptionCommand, bool>
    {
        private readonly IRepository<Subscription> _repository;

        public UpdateSubscriptionCommandHandler(IRepository<Subscription> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(UpdateSubscriptionCommand request, CancellationToken cancellationToken)
        {
            var subscription = await _repository.GetByIdAsync(request.Id);

            if (subscription == null || subscription.UserId != request.UserId)
            {
                return false; // Not found or not owned by user
            }

            subscription.Name = request.Name;
            subscription.Amount = request.Amount;
            subscription.Currency = request.Currency;
            subscription.BillingCycle = request.BillingCycle;
            subscription.NextPaymentDate = request.NextPaymentDate;
            subscription.Category = request.Category;

            await _repository.UpdateAsync(subscription);
            return true;
        }
    }
}
