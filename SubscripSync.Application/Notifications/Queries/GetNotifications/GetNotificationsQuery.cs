using MediatR;
using SubscripSync.Domain.Entities;
using SubscripSync.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SubscripSync.Application.Notifications.Queries.GetNotifications
{
    public class GetNotificationsQuery : IRequest<List<NotificationDto>>
    {
        public Guid UserId { get; set; }
    }

    public class NotificationDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public NotificationType Type { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class GetNotificationsQueryHandler : IRequestHandler<GetNotificationsQuery, List<NotificationDto>>
    {
        private readonly INotificationRepository _repository;

        public GetNotificationsQueryHandler(INotificationRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<NotificationDto>> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
        {
            var notifications = await _repository.GetByUserIdAsync(request.UserId);
            
            var dtos = new List<NotificationDto>();
            foreach (var notif in notifications)
            {
                dtos.Add(new NotificationDto
                {
                    Id = notif.Id,
                    UserId = notif.UserId,
                    Title = notif.Title,
                    Message = notif.Message,
                    Type = notif.Type,
                    IsRead = notif.IsRead,
                    CreatedAt = notif.CreatedAt
                });
            }
            return dtos;
        }
    }
}
