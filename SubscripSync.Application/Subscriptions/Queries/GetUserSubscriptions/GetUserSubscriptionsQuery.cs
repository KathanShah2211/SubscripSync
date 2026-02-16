using MediatR;
using SubscripSync.Domain.Entities;
using SubscripSync.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SubscripSync.Application.Subscriptions.Queries.GetUserSubscriptions
{
    public class GetUserSubscriptionsQuery : IRequest<List<SubscriptionDto>>
    {
        public Guid UserId { get; set; }
    }

    public class SubscriptionDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public BillingCycle BillingCycle { get; set; }
        public DateTime NextPaymentDate { get; set; }
        public string Category { get; set; }
        public bool IsActive { get; set; }
    }

    public class GetUserSubscriptionsQueryHandler : IRequestHandler<GetUserSubscriptionsQuery, List<SubscriptionDto>>
    {
        private readonly ISubscriptionRepository _repository;

        public GetUserSubscriptionsQueryHandler(ISubscriptionRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<SubscriptionDto>> Handle(GetUserSubscriptionsQuery request, CancellationToken cancellationToken)
        {
            var subscriptions = await _repository.GetByUserIdAsync(request.UserId);
            
            // Manual mapping for now, consider AutoMapper later
            var dtos = new List<SubscriptionDto>();
            foreach (var sub in subscriptions)
            {
                dtos.Add(new SubscriptionDto
                {
                    Id = sub.Id,
                    Name = sub.Name,
                    Amount = sub.Amount,
                    Currency = sub.Currency,
                    BillingCycle = sub.BillingCycle,
                    NextPaymentDate = sub.NextPaymentDate,
                    Category = sub.Category,
                    IsActive = sub.IsActive
                });
            }
            return dtos;
        }
    }
}
