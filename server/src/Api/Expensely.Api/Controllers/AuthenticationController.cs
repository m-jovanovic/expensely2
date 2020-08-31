using System.Threading.Tasks;
using Expensely.Api.Contracts;
using Expensely.Api.Infrastructure;
using Expensely.Application.Authentication.Commands.Login;
using Expensely.Application.Authentication.Commands.Register;
using Expensely.Application.Contracts.Authentication;
using Expensely.Domain;
using Expensely.Domain.Core;
using Expensely.Domain.Core.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Expensely.Api.Controllers
{
    [AllowAnonymous]
    public class AuthenticationController : ApiController
    {
        public AuthenticationController(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpPost(ApiRoutes.Authentication.Register)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest? request) =>
            await Result.Create(request, Errors.General.BadRequest)
                .Map(value => new RegisterCommand(
                    value.FirstName,
                    value.LastName,
                    value.Email,
                    value.Password,
                    value.ConfirmPassword))
                .Bind(command => Mediator.Send(command))
                .Match(Ok, e => BadRequest(e));

        [HttpPost(ApiRoutes.Authentication.Login)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginRequest? request) =>
            await Result.Create(request, Errors.General.BadRequest)
                .Map(value => new LoginCommand(value.Email, value.Password))
                .Bind(command => Mediator.Send(command))
                .Match(Ok, e => BadRequest(e));
    }
}
