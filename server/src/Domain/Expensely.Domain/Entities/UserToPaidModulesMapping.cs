using System;
using Expensely.Domain.Enums;

namespace Expensely.Domain.Entities
{
    /// <summary>
    /// Represents the User-to-PaidModules mapping.
    /// </summary>
    public sealed class UserToPaidModulesMapping
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserToPaidModulesMapping"/> class.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="paidModules">The paid modules.</param>
        public UserToPaidModulesMapping(Guid userId, PaidModules paidModules)
            : this()
        {
            UserId = userId;
            PaidModules = paidModules;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserToPaidModulesMapping"/> class.
        /// </summary>
        /// <remarks>
        /// Required by EF Core.
        /// </remarks>
        private UserToPaidModulesMapping()
        {
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
