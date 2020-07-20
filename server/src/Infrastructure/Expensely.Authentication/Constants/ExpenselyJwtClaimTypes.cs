using Expensely.Common.Authorization.Permissions;

namespace Expensely.Authentication.Constants
{
    public static class ExpenselyJwtClaimTypes
    {
        public const string UserId = "UserId";

        public const string Email = "Email";

        public const string Name = "Name";

        public const string Permissions = PermissionConstants.PermissionsClaimType;
    }
}
