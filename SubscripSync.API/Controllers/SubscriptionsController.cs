using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SubscripSync.Application.Subscriptions.Commands.CreateSubscription;
using SubscripSync.Application.Subscriptions.Commands.UpdateSubscription;
using SubscripSync.Application.Subscriptions.Commands.DeleteSubscription;
using SubscripSync.Application.Subscriptions.Queries.GetUserSubscriptions;
using System;
using System.Threading.Tasks;

namespace SubscripSync.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SubscriptionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSubscriptionCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(id);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(Guid userId)
        {
            // Security: Ensure user can only access their own subscriptions
            // Extract userId from token
            var tokenUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            
            if (tokenUserId != null && Guid.Parse(tokenUserId) != userId)
            {
                return Forbid();
            }

            var query = new GetUserSubscriptionsQuery { UserId = userId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateSubscriptionCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("ID mismatch");
            }

            var tokenUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (tokenUserId == null) return Unauthorized();

            command.UserId = Guid.Parse(tokenUserId);

            var result = await _mediator.Send(command);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var tokenUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (tokenUserId == null) return Unauthorized();

            var command = new DeleteSubscriptionCommand 
            { 
                Id = id, 
                UserId = Guid.Parse(tokenUserId) 
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
