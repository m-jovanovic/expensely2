using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Expensely.Infrastructure.Authentication.Constants
{
    /// <summary>
    /// Contains default values for configuring JWT authentication.
    /// </summary>
    internal static class ExpenselyJwtDefaults
    {
        internal const string AuthenticationScheme = JwtBearerDefaults.AuthenticationScheme;

        internal const string IssuerSettingsKey = "Jwt:Issuer";

        internal const string AudienceSettingsKey = "Jwt:Audience";

        internal const string SecurityKeySettingsKey = "Jwt:SecurityKey";
    }
}
