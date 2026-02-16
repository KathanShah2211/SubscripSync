using MediatR;
using Microsoft.AspNetCore.Mvc;
using SubscripSync.Application.Users.Commands.CreateUser;
using System.Threading.Tasks;

namespace SubscripSync.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(id);
        }
    }
}
