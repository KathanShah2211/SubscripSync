using MediatR;
using SubscripSync.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SubscripSync.Application.Notifications.Commands.MarkNotificationAsRead
{
    public class MarkNotificationAsReadCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
    }

    public class MarkNotificationAsReadCommandHandler : IRequestHandler<MarkNotificationAsReadCommand, bool>
    {
        private readonly INotificationRepository _repository;

        public MarkNotificationAsReadCommandHandler(INotificationRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(MarkNotificationAsReadCommand request, CancellationToken cancellationToken)
        {
            var notification = await _repository.GetByIdAsync(request.Id);

            if (notification == null || notification.UserId != request.UserId)
            {
                return false;
            }

            notification.IsRead = true;
            await _repository.UpdateAsync(notification);
            
            return true;
        }
    }
}
