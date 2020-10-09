using Expensely.Api.Contracts;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Core.Result;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Expensely.Api.Infrastructure
{
    [Authorize]
    public abstract class AsyncEndpoint : ControllerBase
    {
        protected AsyncEndpoint(IMediator mediator) => Mediator = mediator;

        public IMediator Mediator { get; }

        protected new IActionResult Ok(object value) => base.Ok(value);

        protected new IActionResult NotFound() => base.NotFound();

        /// <summary>
        /// Creates an <see cref="BadRequestObjectResult"/> that produces a <see cref="StatusCodes.Status400BadRequest"/>.
        /// response based on the specified <see cref="Result"/>.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <returns>The created <see cref="BadRequestObjectResult"/> for the response.</returns>
        protected IActionResult BadRequest(Error error) => BadRequest(new ApiErrorResponse(new[] { error }));
    }
}
