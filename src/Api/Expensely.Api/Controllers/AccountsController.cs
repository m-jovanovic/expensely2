using System.Threading.Tasks;
using Expensely.Authentication.Abstractions;
using Expensely.Common.Contracts.Authentication;
using Expensely.Domain.Core.Primitives;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Expensely.Api.Controllers
{
    [Route("api/accounts")]
    [AllowAnonymous]
    public class AccountsController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AccountsController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody]RegisterRequest request)
        {
            Result result = await _authenticationService.RegisterAsync(request);

            if (result.IsFailure)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
