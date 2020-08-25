using System;

namespace Expensely.Domain.Authorization
{
    /// <summary>
    /// Represents the User-to-PaidModules mapping.
    /// </summary>
    public sealed class UserPaidModules
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserPaidModules"/> class.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="paidModules">The paid modules.</param>
        public UserPaidModules(Guid userId, PaidModules paidModules)
            : this()
        {
            UserId = userId;
            PaidModules = paidModules;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserPaidModules"/> class.
        /// </summary>
        /// <remarks>
        /// Required by EF Core.
        /// </remarks>
        private UserPaidModules()
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
