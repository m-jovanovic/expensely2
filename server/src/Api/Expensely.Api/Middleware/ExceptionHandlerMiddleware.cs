using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Expensely.Api.Contracts;
using Expensely.Application.Exceptions;
using Expensely.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Expensely.Api.Middleware
{
    /// <summary>
    /// Represents the exception handler middleware.
    /// </summary>
    internal class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionHandlerMiddleware"/> class.
        /// </summary>
        /// <param name="next">The delegate pointing to the next middleware in the chain.</param>
        /// <param name="logger">The logger.</param>
        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Invokes the exception handler middleware with the specified <see cref="HttpContext"/>.
        /// </summary>
        /// <param name="httpContext">The HTTP httpContext.</param>
        /// <returns>The task that can be awaited by the next middleware.</returns>
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occurred: {Message}", ex.Message);

                await HandleExceptionAsync(httpContext, ex);
            }
        }

        /// <summary>
        /// Handles the specified <see cref="Exception"/> for the specified <see cref="HttpContext"/>.
        /// </summary>
        /// <param name="httpContext">The HTTP httpContext.</param>
        /// <param name="exception">The exception.</param>
        /// <returns>The HTTP response that is modified based on the exception.</returns>
        private static Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;

            string result = string.Empty;

            var serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            if (exception is ValidationException validationException)
            {
                httpStatusCode = HttpStatusCode.BadRequest;

                result = JsonSerializer.Serialize(new ApiErrorResponse(validationException.ErrorCodes), serializerOptions);
            }

            httpContext.Response.ContentType = "application/json";

            httpContext.Response.StatusCode = (int)httpStatusCode;

            if (result.Length == 0)
            {
                result = JsonSerializer.Serialize(new ApiErrorResponse(new[] { Errors.General.ServerError }), serializerOptions);
            }

            return httpContext.Response.WriteAsync(result);
        }
    }

    /// <summary>
    /// Contains extension methods for configuring the exception handler middleware.
    /// </summary>
    internal static class ExceptionHandlerMiddlewareExtensions
    {
        /// <summary>
        /// Configure the custom exception handler middleware.
        /// </summary>
        /// <param name="builder">The application builder.</param>
        /// <returns>The configured application builder.</returns>
        internal static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
            => builder.UseMiddleware<ExceptionHandlerMiddleware>();
    }
}
