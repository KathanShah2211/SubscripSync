using MediatR;
using Microsoft.AspNetCore.Mvc;
using SubscripSync.Application.Authentication.Commands.Register;
using SubscripSync.Application.Authentication.Common;
using SubscripSync.Application.Authentication.Queries.Login;
using System.Threading.Tasks;

namespace SubscripSync.API.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ISender _mediator;

        public AuthenticationController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var command = new RegisterCommand(
                request.FirstName,
                request.LastName,
                request.Email,
                request.Password);

            try
            {
                var result = await _mediator.Send(command);
                var response = new AuthResponse(
                    result.User.Id,
                    result.User.FirstName,
                    result.User.LastName,
                    result.User.Email,
                    result.Token);

                return Ok(response);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var query = new LoginQuery(request.Email, request.Password);

            try
            {
                var result = await _mediator.Send(query);
                var response = new AuthResponse(
                    result.User.Id,
                    result.User.FirstName,
                    result.User.LastName,
                    result.User.Email,
                    result.Token);

                return Ok(response);
            }
            catch (System.Exception ex)
            {
                // In production, don't return specific error messages for security
                return Unauthorized(new { error = ex.Message });
            }
        }
    }

    public record RegisterRequest(
        string FirstName,
        string LastName,
        string Email,
        string Password);

    public record LoginRequest(
        string Email,
        string Password);
}
