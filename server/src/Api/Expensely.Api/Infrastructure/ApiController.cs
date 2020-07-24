﻿using Expensely.Api.Contracts;
using Expensely.Domain.Core.Primitives;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        /// <summary>
        /// Creates an <see cref="BadRequestObjectResult"/> that produces a <see cref="StatusCodes.Status400BadRequest"/> response.
        /// </summary>
        /// <summary>
        /// Creates <see cref="BadRequestObjectResult"/> that produces a  based on the specified <see cref="Result"/>.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns>The created <see cref="BadRequestObjectResult"/> for the response.</returns>
        protected IActionResult BadRequest(Result result)
        {
            return BadRequest(new ApiErrorResponse(new[] { result.Error }));
        }
    }
}
