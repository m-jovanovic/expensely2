namespace Expensely.Authentication.Options
{
    public class JwtOptions
    {
        public const string SettingsKey = "Jwt";

        public JwtOptions()
        {
            Issuer = string.Empty;
            Audience = string.Empty;
            SecurityKey = string.Empty;
        }

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string SecurityKey { get; set; }

        public int TokenExpirationInMinutes { get; set; }
    }
}
