using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubscripSync.Application.Notifications.Queries.GetNotifications;
using SubscripSync.Application.Notifications.Commands.MarkNotificationAsRead;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SubscripSync.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotificationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        private Guid GetUserId()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(userIdString, out var userId))
            {
                return userId;
            }
            throw new UnauthorizedAccessException("User ID not found in token.");
        }

        [HttpGet]
        public async Task<IActionResult> GetNotifications()
        {
            var query = new GetNotificationsQuery { UserId = GetUserId() };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPut("{id}/read")]
        public async Task<IActionResult> MarkAsRead(Guid id)
        {
            var command = new MarkNotificationAsReadCommand
            {
                Id = id,
                UserId = GetUserId()
            };
            
            var result = await _mediator.Send(command);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
