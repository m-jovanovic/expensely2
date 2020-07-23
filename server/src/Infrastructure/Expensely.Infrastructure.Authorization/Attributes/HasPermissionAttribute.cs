using System;
using Microsoft.AspNetCore.Authorization;

namespace Expensely.Infrastructure.Authorization.Attributes
{
    /// <summary>
    /// Represents the permission authorization attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false)]
    public sealed class HasPermissionAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HasPermissionAttribute"/> class.
        /// </summary>
        /// <param name="requiredPermission">The required permission.</param>
        public HasPermissionAttribute(Permission requiredPermission)
            : base(requiredPermission.ToString())
        {
        }
    }
}
