using System;
using Expensely.Api.Contracts;
using Expensely.Domain;
using Expensely.Domain.Core.Primitives;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Expensely.Api.Infrastructure
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
        protected IMediator Mediator => (_mediator ??= HttpContext.RequestServices.GetService<IMediator>()) ??
                                        throw new ArgumentNullException(nameof(Mediator));

        /// <summary>
        /// Creates an <see cref="BadRequestObjectResult"/> that produces a <see cref="StatusCodes.Status400BadRequest"/>
        /// response with a generic bad request error.
        /// </summary>
        /// <returns>The created <see cref="BadRequestObjectResult"/> for the response.</returns>
        protected new IActionResult BadRequest()
        {
            return BadRequest(Result.Fail(Errors.General.BadRequest));
        }

        /// <summary>
        /// Creates an <see cref="BadRequestObjectResult"/> that produces a <see cref="StatusCodes.Status400BadRequest"/>
        /// response based on the specified <see cref="Result"/>.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns>The created <see cref="BadRequestObjectResult"/> for the response.</returns>
        protected IActionResult BadRequest(Result result)
        {
            return BadRequest(new ApiErrorResponse(new[] { result.Error }));
        }
    }
}
