using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SubscripSync.Application.Subscriptions.Commands.CreateSubscription;
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
            var query = new GetUserSubscriptionsQuery { UserId = userId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
