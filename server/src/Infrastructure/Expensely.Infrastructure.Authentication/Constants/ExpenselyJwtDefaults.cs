using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Expensely.Infrastructure.Authentication.Constants
{
    public static class ExpenselyJwtDefaults
    {
        public const string AuthenticationScheme = JwtBearerDefaults.AuthenticationScheme;

        public const string IssuerSettingsKey = "Jwt:Issuer";

        public const string AudienceSettingsKey = "Jwt:Audience";

        public const string SecurityKeySettingsKey = "Jwt:SecurityKey";
    }
}
