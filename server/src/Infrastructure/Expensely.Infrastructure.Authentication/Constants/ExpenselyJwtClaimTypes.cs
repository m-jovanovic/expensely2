using Expensely.Infrastructure.Authorization.Permissions;

namespace Expensely.Infrastructure.Authentication.Constants
{
    public static class ExpenselyJwtClaimTypes
    {
        public const string UserId = "userId";

        public const string Email = "email";

        public const string Name = "name";

        public const string Permissions = PermissionConstants.PermissionsClaimType;
    }
}
