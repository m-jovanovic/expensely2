using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Expensely.Api.Controllers
{
    /// <summary>
    /// Represents the abstract API controller class that all controllers derive from.
    /// </summary>
    [Authorize]
    public abstract class ApiController : ControllerBase
    {
        private IMediator? _mediator;

        /// <summary>
        /// Gets the <see cref="IMediator"/> instance.
        /// </summary>
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
    }
}
