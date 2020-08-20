using System;
using System.Security.Claims;
using Expensely.Application.Core.Abstractions.Authentication;
using Expensely.Infrastructure.Authentication.Constants;
using Microsoft.AspNetCore.Http;

namespace Expensely.Infrastructure.Authentication.Providers
{
    /// <summary>
    /// Represents the user identifier provider.
    /// </summary>
    internal sealed class UserIdentifierProvider : IUserIdentifierProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserIdentifierProvider"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public UserIdentifierProvider(IHttpContextAccessor httpContextAccessor)
        {
            string userIdClaim = httpContextAccessor.HttpContext?.User?.FindFirstValue(ExpenselyJwtClaimTypes.UserId)
                ?? throw new ArgumentException("The user identifier claim is required.", nameof(httpContextAccessor));

            UserId = new Guid(userIdClaim);
        }

        /// <inheritdoc />
        public Guid UserId { get; }
    }
}
