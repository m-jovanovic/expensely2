using System.Collections.Generic;
using Expensely.Domain.Core;

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
            Errors = errors;
        }

        /// <summary>
        /// Gets the errors.
        /// </summary>
        public IEnumerable<Error> Errors { get; }
    }
}
