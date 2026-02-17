using MediatR;
using SubscripSync.Domain.Entities;
using SubscripSync.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SubscripSync.Application.Subscriptions.Commands.DeleteSubscription
{
    public class DeleteSubscriptionCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
    }

    public class DeleteSubscriptionCommandHandler : IRequestHandler<DeleteSubscriptionCommand, bool>
    {
        private readonly IRepository<Subscription> _repository;

        public DeleteSubscriptionCommandHandler(IRepository<Subscription> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteSubscriptionCommand request, CancellationToken cancellationToken)
        {
            var subscription = await _repository.GetByIdAsync(request.Id);

            if (subscription == null || subscription.UserId != request.UserId)
            {
                return false;
            }

            // Soft delete or Hard delete?
            // The requirement didn't specify, but I'll go with Hard Delete for now as per Repository interface.
            // If repository has DeleteAsync, it likely removes it.
            // Alternatively, we could set IsActive = false.
            // But let's follow the CRUD pattern.

            await _repository.DeleteAsync(subscription);
            return true;
        }
    }
}
