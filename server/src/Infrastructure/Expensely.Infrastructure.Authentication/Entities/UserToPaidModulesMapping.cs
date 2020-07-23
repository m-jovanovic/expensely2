using System;
using Expensely.Infrastructure.Authorization;

namespace Expensely.Infrastructure.Authentication.Entities
{
    /// <summary>
    /// Represents the User-to-PaidModules mapping.
    /// </summary>
    internal sealed class UserToPaidModulesMapping
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserToPaidModulesMapping"/> class.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="paidModules">The paid modules.</param>
        public UserToPaidModulesMapping(Guid userId, PaidModules paidModules)
        {
            UserId = userId;
            PaidModules = paidModules;
        }

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public Guid UserId { get; }

        /// <summary>
        /// Gets the paid modules.
        /// </summary>
        public PaidModules PaidModules { get; }
    }
}
