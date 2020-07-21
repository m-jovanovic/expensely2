using System;
using Microsoft.AspNetCore.Authorization;

namespace Expensely.Infrastructure.Authorization.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false)]
    public class HasPermissionAttribute : AuthorizeAttribute
    {
        public HasPermissionAttribute(Permission permission)
            : base(permission.ToString())
        {
        }
    }
}
