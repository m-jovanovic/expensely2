using Expensely.Infrastructure.Authentication.Permissions;

namespace Expensely.Infrastructure.Authentication.Constants
{
    /// <summary>
    /// Contains the application JWT claim types.
    /// </summary>
    internal static class ExpenselyJwtClaimTypes
    {
        internal const string UserId = "userId";

        internal const string Email = "email";

        internal const string Name = "name";

        internal const string Permissions = PermissionConstants.PermissionsClaimType;
    }
}
