using System.Threading.Tasks;
using Expensely.Authentication.Interfaces;
using Expensely.Contracts.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Expensely.Api.Controllers
{
    [Route("api/accounts")]
    public class AccountsController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AccountsController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(RegisterUserResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Register([FromBody]RegisterUserRequest request) =>
            Ok(await _authenticationService.RegisterUserAsync(request.Email, request.Password));
    }
}
