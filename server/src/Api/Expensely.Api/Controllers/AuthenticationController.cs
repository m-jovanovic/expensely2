using System.Threading.Tasks;
using Expensely.Application.Authentication.Commands.Login;
using Expensely.Application.Authentication.Commands.Register;
using Expensely.Application.Contracts.Authentication;
using Expensely.Domain.Core.Primitives;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Expensely.Api.Controllers
{
    [Route("api/authentication")]
    public class AuthenticationController : ApiController
    {
        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var registerCommand = new RegisterCommand(
                request.FirstName,
                request.LastName,
                request.Email,
                request.Password,
                request.ConfirmPassword);

            Result<string> result = await Mediator.Send(registerCommand);

            if (result.IsFailure)
            {
                return BadRequest(result);
            }

            return Ok(result.Value());
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var loginCommand = new LoginCommand(request.Email, request.Password);

            Result<string> result = await Mediator.Send(loginCommand);

            if (result.IsFailure)
            {
                return BadRequest(result);
            }

            return Ok(result.Value());
        }
    }
}
