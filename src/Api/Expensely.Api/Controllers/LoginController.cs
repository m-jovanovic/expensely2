using System.Threading.Tasks;
using Expensely.Authentication.Abstractions;
using Expensely.Common.Contracts.Authentication;
using Expensely.Domain.Core.Primitives;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Expensely.Api.Controllers
{
    [Route("api/login")]
    [AllowAnonymous]
    public class LoginController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public LoginController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody]LoginRequest request)
        {
            Result result = await _authenticationService.LoginAsync(request);

            if (result.IsFailure)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
