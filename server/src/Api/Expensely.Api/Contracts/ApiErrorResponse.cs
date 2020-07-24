using System.Collections.Generic;
using Expensely.Domain.Core.Primitives;

namespace Expensely.Api.Contracts
{
    /// <summary>
    /// Represents API an error response.
    /// </summary>
    public class ApiErrorResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiErrorResponse"/> class.
        /// </summary>
        /// <param name="errors">The enumerable collection of errors.</param>
        public ApiErrorResponse(IEnumerable<Error> errors)
        {
            Success = false;
            Errors = errors;
        }

        /// <summary>
        /// Gets a value indicating whether the response is a success response. Always returns false.
        /// </summary>
        public bool Success { get; }

        /// <summary>
        /// Gets the errors.
        /// </summary>
        public IEnumerable<Error> Errors { get; }
    }
}
